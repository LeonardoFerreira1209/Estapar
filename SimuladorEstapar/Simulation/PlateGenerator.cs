namespace SimulatorEstapar.Simulation;

/// <summary>
/// Generates random license plates following the Brazilian Mercosul format (LLLNLNN).
/// </summary>
public static class PlateGenerator
{
    private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// Generates a new random plate, e.g. <c>ABC1D23</c>.
    /// </summary>
    public static string Generate()
    {
        var random = Random.Shared;

        Span<char> plate = stackalloc char[7];

        plate[0] = Letters[random.Next(Letters.Length)];
        plate[1] = Letters[random.Next(Letters.Length)];
        plate[2] = Letters[random.Next(Letters.Length)];
        plate[3] = (char)('0' + random.Next(10));
        plate[4] = Letters[random.Next(Letters.Length)];
        plate[5] = (char)('0' + random.Next(10));
        plate[6] = (char)('0' + random.Next(10));

        return new string(plate);
    }
}
