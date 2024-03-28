namespace FF6WCToolsLib.DataTemplates;

public struct StatBoostValues
{
    private int vigorBoost;
    private int speedBoost;
    private int staminaBoost;
    private int magicBoost;


    public StatBoostValues(int vigor, int speed, int stamina, int magic)
    {
        vigorBoost = vigor;
        speedBoost = speed;
        staminaBoost = stamina;
        magicBoost = magic;
    }

    public bool HasChanged (StatBoostValues old, out StatBoostValues difference)
    {
        bool hasChanged;
        difference = new();

        if (old.vigorBoost == vigorBoost && old.speedBoost == speedBoost &&
            old.staminaBoost == staminaBoost && old.magicBoost == magicBoost)
        {
            hasChanged = false;
        }
        else
        {
            // New item has different boost values.
            difference = new()
            {
                vigorBoost = vigorBoost - old.vigorBoost,
                speedBoost = speedBoost - old.speedBoost,
                staminaBoost = staminaBoost - old.staminaBoost,
                magicBoost = magicBoost - old.magicBoost 
            };

            hasChanged = true;
        }

        return hasChanged;
    }

    public override string ToString()
    {
        return $"Vigor: {vigorBoost}, Speed: {speedBoost}, Stamina: {staminaBoost}, Magic: {magicBoost}";
    }
}
