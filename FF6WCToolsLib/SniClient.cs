using System;
using Google.Protobuf;
using Grpc.Net.Client;

namespace FF6WCToolsLib;

/// <summary>
/// A class for handling the connection to SNI and requesting memory reads.
/// </summary>
public class SniClient
{
    public event EventHandler<ConnectionSuccessfulEventArgs>? OnConnectionSuccessful;
    public event EventHandler<ConnectionErrorEventArgs>? OnConnectionError;
    
    private const string SniAddress = "http://localhost:8191/";
    private readonly GrpcChannel _sniChannel = GrpcChannel.ForAddress(SniAddress);

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

        _isValidConnection = false;
        RequestTimer = 0;
    }

    /// <summary>
    /// Method that attempts to retrieve the first device connected to SNI.
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
        catch (ArgumentOutOfRangeException)
        {
            while (!_isValidConnection)
            {
                try
                {
                    string message = "Device not found! Make sure your device/emulator is correctly connected to SNI.";
                    OnConnectionError?.Invoke(this, new ConnectionErrorEventArgs(message));
                    var firstDevice = _devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
                    _isValidConnection = true;
                }
                catch (Exception) { }
            }
            ResetConnection();
        }
        catch (Grpc.Core.RpcException)
        {
            while (!_isValidConnection)
            {
                try
                {
                    string message = "Error - SNI not found! Make sure it's open and connected to your device/emulator.";
                    OnConnectionError?.Invoke(this, new ConnectionErrorEventArgs(message));
                    var firstDevice = _devicesClient.ListDevices(new DevicesRequest { }).Devices[0];
                    _isValidConnection = true;
                }
                catch (Exception) { }
            }
            ResetConnection();
        }
    }

    /// <summary>
    /// Method that reads a specific memory address in the game through the gRPC API and returns its value.
    /// </summary>
    /// <param name="address">The memory address to read.</param>
    /// <param name="size">The size of the request in bytes.</param>
    /// <returns>A byte array containing the data of each byte.</returns>
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