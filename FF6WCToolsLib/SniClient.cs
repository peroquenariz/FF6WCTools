using System;
using System.Net;
using System.Threading;
using FF6WCToolsLib.DataTemplates;
using Google.Protobuf;
using Grpc.Net.Client;

namespace FF6WCToolsLib;

/// <summary>
/// Handles the connection to SNI.
/// </summary>
public class SniClient
{
    public event EventHandler<ConnectionSuccessfulEventArgs>? OnConnectionSuccessful;
    public event EventHandler<ConnectionErrorEventArgs>? OnConnectionError;
    public event EventHandler<CountdownEventArgs>? OnCountdownTick;
    
    private const string SNI_ADDRESS = "http://localhost:8191/";
    private readonly GrpcChannel _sniChannel = GrpcChannel.ForAddress(SNI_ADDRESS);

    // Devices client.
    private readonly Devices.DevicesClient _devicesClient;

    // Memory client
    private readonly DeviceMemory.DeviceMemoryClient _memoryClient;

    // Reading or writing memory requires passing 2 messages to the service.
    private readonly ReadMemoryRequest _readMemoryRequest;
    private readonly SingleReadMemoryRequest _singleReadMemoryRequest;
    private readonly WriteMemoryRequest _writeMemoryRequest;
    private readonly SingleWriteMemoryRequest _singleWriteMemoryRequest;

    private bool _isValidConnection;

    public int RequestTimer { get; set; }

    public SniClient()
    {
        _devicesClient = new Devices.DevicesClient(_sniChannel);

        _memoryClient = new DeviceMemory.DeviceMemoryClient(_sniChannel);

        _readMemoryRequest = new ReadMemoryRequest
        {
            RequestMemoryMapping = MemoryMapping.HiRom,
        };

        _singleReadMemoryRequest = new SingleReadMemoryRequest
        {
            Uri = "",
            Request = _readMemoryRequest
        };

        _writeMemoryRequest = new WriteMemoryRequest
        {
            RequestMemoryMapping = MemoryMapping.HiRom,

        };

        _singleWriteMemoryRequest = new SingleWriteMemoryRequest
        {
            Uri = "",
            Request = _writeMemoryRequest
        };

        RequestTimer = 0;
    }

    /// <summary>
    /// Attempts to retrieve the first device connected to SNI.
    /// If it succeeds, it will set the request URI for that device.
    /// Otherwise, it will keep retrying until it succeeds.
    /// </summary>
    public void ResetConnection()
    {
        try
        {
            var firstDevice = _devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
            _singleReadMemoryRequest.Uri = firstDevice.Uri;
            _singleWriteMemoryRequest.Uri = firstDevice.Uri;
            _readMemoryRequest.RequestAddressSpace = AddressSpace.SnesAbus;
            _writeMemoryRequest.RequestAddressSpace = AddressSpace.SnesAbus;
            _isValidConnection = true;
            OnConnectionSuccessful?.Invoke(this, new ConnectionSuccessfulEventArgs(firstDevice.Uri));
        }
        catch (Exception e)
        {
            string exceptionType = e.GetType().ToString();
            while (!_isValidConnection)
            {
                string errorMessage;
                
                switch (exceptionType)
                {
                    case "System.ArgumentOutOfRangeException":
                        errorMessage = "Error: Device not found! Make sure your device/emulator is correctly connected to SNI.";
                        break;
                    case "Grpc.Core.RpcException":
                        errorMessage = "Error: SNI not found! Make sure it's running and connected to your device/emulator.";
                        break;
                    default:
                        throw;
                }

                try
                {
                    OnConnectionError?.Invoke(this, new ConnectionErrorEventArgs(errorMessage));
                    for (int i = 5; i > 0; i--)
                    {
                        OnCountdownTick?.Invoke(this, new CountdownEventArgs(i));
                        Thread.Sleep(1000);
                    }
                    var firstDevice = _devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
                    _isValidConnection = true;
                }
                catch (Exception ex)
                {
                    exceptionType = ex.GetType().ToString();
                }
            }
            ResetConnection();
        }
    }

    /// <summary>
    /// Reads a specific game memory section and returns its value.
    /// </summary>
    /// <param name="address">The memory address to read.</param>
    /// <param name="size">The size of the request in bytes.</param>
    /// <returns>A byte array containing the game memory data requested.</returns>
    public byte[] ReadMemory(uint address, uint size)
    {
        try
        {
            _readMemoryRequest.RequestAddress = address;
            _readMemoryRequest.Size = size;
            byte[] response = _memoryClient.SingleRead(_singleReadMemoryRequest).Response.Data.Memory.ToArray();
            return response;
        }
        catch
        {
            _isValidConnection = false;
            ResetConnection();
            return ReadMemory(address, size);
        }
    }

    /// <summary>
    /// Takes a byte array and writes it to memory.
    /// </summary>
    /// <param name="address">The memory address to write to.</param>
    /// <param name="data">The data to write.</param>
    public void WriteMemory(uint address, byte[] data)
    {
        try
        {
            _writeMemoryRequest.RequestAddress = address;
            _writeMemoryRequest.Data = ByteString.CopyFrom(data);
            _memoryClient.SingleWrite(_singleWriteMemoryRequest);
            return;
        }
        catch
        {
            _isValidConnection = false;
            ResetConnection();
            WriteMemory(address, data);
        }
    }

    /// <summary>
    /// Takes a byte array and writes it to memory.
    /// </summary>
    /// <param name="memoryBlock">A writable memory block.</param>
    public void WriteMemory(IWritableMemoryBlock memoryBlock)
    {
        try
        {
            _writeMemoryRequest.RequestAddress = memoryBlock.TargetAddress;
            _writeMemoryRequest.Data = ByteString.CopyFrom(memoryBlock.ToByteArray());
            _memoryClient.SingleWrite(_singleWriteMemoryRequest);
            return;
        }
        catch
        {
            _isValidConnection = false;
            ResetConnection();
            WriteMemory(memoryBlock);
        }
    }
}

public class CountdownEventArgs : EventArgs
{
    public int Counter { get; }
    public CountdownEventArgs(int counter)
    {
        Counter = counter;
    }
}

public class ConnectionSuccessfulEventArgs : EventArgs
{
    public string Uri { get; }
    public ConnectionSuccessfulEventArgs(string uri)
    {
        Uri = uri;
    }
}

public class ConnectionErrorEventArgs : EventArgs
{
    public string Message { get; }
    public ConnectionErrorEventArgs(string message)
    {
        Message = message;
    }
}