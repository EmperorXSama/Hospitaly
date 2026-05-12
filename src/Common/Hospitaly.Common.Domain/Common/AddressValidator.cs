namespace Hospitaly.Common.Domain.Common;

public class AddressValidator
{
   private static readonly Dictionary<string, HashSet<string>> RegionCities = new()
    {
        ["Tanger-Tetouan-Al Hoceima"] = new()
        {
            "Tanger",
            "Tetouan",
            "Al Hoceima",
            "Larache",
            "Chefchaouen",
            "Ouazzane",
            "M'Diq",
            "Fnideq",
            "Ksar El Kebir",
            "Asilah"
        },

        ["Oriental"] = new()
        {
            "Oujda",
            "Nador",
            "Berkane",
            "Taourirt",
            "Jerada",
            "Figuig",
            "Driouch",
            "Guercif",
            "Saïdia",
            "Ahfir"
        },

        ["Fes-Meknes"] = new()
        {
            "Fes",
            "Meknes",
            "Ifrane",
            "Taza",
            "Sefrou",
            "Boulemane",
            "El Hajeb",
            "Moulay Yacoub",
            "Taounate",
            "Azrou"
        },

        ["Rabat-Sale-Kenitra"] = new()
        {
            "Rabat",
            "Sale",
            "Kenitra",
            "Temara",
            "Skhirat",
            "Khemisset",
            "Sidi Kacem",
            "Sidi Slimane",
            "Tiflet",
            "Mechra Bel Ksiri"
        },

        ["Beni Mellal-Khenifra"] = new()
        {
            "Beni Mellal",
            "Khenifra",
            "Khouribga",
            "Fquih Ben Salah",
            "Azilal",
            "Kasba Tadla",
            "Oued Zem",
            "Boujad",
            "Souk Sebt",
            "Demnate"
        },

        ["Casablanca-Settat"] = new()
        {
            "Casablanca",
            "Mohammedia",
            "Settat",
            "El Jadida",
            "Berrechid",
            "Mediouna",
            "Nouaceur",
            "Benslimane",
            "Sidi Bennour",
            "Azemmour"
        },

        ["Marrakech-Safi"] = new()
        {
            "Marrakech",
            "Safi",
            "Essaouira",
            "El Kelaa des Sraghna",
            "Chichaoua",
            "Al Haouz",
            "Youssoufia",
            "Rehamna",
            "Ben Guerir",
            "Imintanoute"
        },

        ["Draa-Tafilalet"] = new()
        {
            "Errachidia",
            "Ouarzazate",
            "Zagora",
            "Tinghir",
            "Midelt",
            "Rissani",
            "Erfoud",
            "Skoura",
            "Agdz",
            "Boumalne Dades"
        },

        ["Souss-Massa"] = new()
        {
            "Agadir",
            "Inezgane",
            "Ait Melloul",
            "Taroudant",
            "Tiznit",
            "Chtouka Ait Baha",
            "Tata",
            "Biougra",
            "Oulad Teima",
            "Sidi Ifni"
        },

        ["Guelmim-Oued Noun"] = new()
        {
            "Guelmim",
            "Tan-Tan",
            "Sidi Ifni",
            "Assa",
            "Zag",
            "Akhfenir",
            "Bouizakarne",
            "Taghjijt",
            "Tighmert"
        },

        ["Laayoune-Sakia El Hamra"] = new()
        {
            "Laayoune",
            "Boujdour",
            "Tarfaya",
            "Es-Semara",
            "El Marsa",
            "Foum El Oued",
            "Akhfennir"
        },

        ["Dakhla-Oued Ed-Dahab"] = new()
        {
            "Dakhla",
            "Aousserd",
            "Bir Gandouz",
            "El Argoub",
            "Guerguerat"
        }
    };

    public static bool IsValidRegion(string region)
    {
        if (string.IsNullOrWhiteSpace(region))
        {
            return false;
        }
        return RegionCities.ContainsKey(region.Trim());
    }
    public static bool IsValidCity(string region , string city)
    {
        if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(city))
        {
            return false;
        }
        if (!RegionCities.TryGetValue(region.Trim(), out var regionCity))
        {
            return false;
        }
        return Enumerable.Contains(regionCity, city.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}