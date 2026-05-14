export const REGION_CITIES: Record<string, string[]> = {
  'Tanger-Tetouan-Al Hoceima': [
    'Tanger',
    'Tetouan',
    'Al Hoceima',
    'Larache',
    'Chefchaouen',
    'Ouazzane',
    "M'Diq",
    'Fnideq',
    'Ksar El Kebir',
    'Asilah',
  ],

  Oriental: [
    'Oujda',
    'Nador',
    'Berkane',
    'Taourirt',
    'Jerada',
    'Figuig',
    'Driouch',
    'Guercif',
    'Saïdia',
    'Ahfir',
  ],

  'Fes-Meknes': [
    'Fes',
    'Meknes',
    'Ifrane',
    'Taza',
    'Sefrou',
    'Boulemane',
    'El Hajeb',
    'Moulay Yacoub',
    'Taounate',
    'Azrou',
  ],

  'Rabat-Sale-Kenitra': [
    'Rabat',
    'Sale',
    'Kenitra',
    'Temara',
    'Skhirat',
    'Khemisset',
    'Sidi Kacem',
    'Sidi Slimane',
    'Tiflet',
    'Mechra Bel Ksiri',
  ],

  'Beni Mellal-Khenifra': [
    'Beni Mellal',
    'Khenifra',
    'Khouribga',
    'Fquih Ben Salah',
    'Azilal',
    'Kasba Tadla',
    'Oued Zem',
    'Boujad',
    'Souk Sebt',
    'Demnate',
  ],

  'Casablanca-Settat': [
    'Casablanca',
    'Mohammedia',
    'Settat',
    'El Jadida',
    'Berrechid',
    'Mediouna',
    'Nouaceur',
    'Benslimane',
    'Sidi Bennour',
    'Azemmour',
  ],

  'Marrakech-Safi': [
    'Marrakech',
    'Safi',
    'Essaouira',
    'El Kelaa des Sraghna',
    'Chichaoua',
    'Al Haouz',
    'Youssoufia',
    'Rehamna',
    'Ben Guerir',
    'Imintanoute',
  ],

  'Draa-Tafilalet': [
    'Errachidia',
    'Ouarzazate',
    'Zagora',
    'Tinghir',
    'Midelt',
    'Rissani',
    'Erfoud',
    'Skoura',
    'Agdz',
    'Boumalne Dades',
  ],

  'Souss-Massa': [
    'Agadir',
    'Inezgane',
    'Ait Melloul',
    'Taroudant',
    'Tiznit',
    'Chtouka Ait Baha',
    'Tata',
    'Biougra',
    'Oulad Teima',
    'Sidi Ifni',
  ],

  'Guelmim-Oued Noun': [
    'Guelmim',
    'Tan-Tan',
    'Sidi Ifni',
    'Assa',
    'Zag',
    'Akhfenir',
    'Bouizakarne',
    'Taghjijt',
    'Tighmert',
  ],

  'Laayoune-Sakia El Hamra': [
    'Laayoune',
    'Boujdour',
    'Tarfaya',
    'Es-Semara',
    'El Marsa',
    'Foum El Oued',
    'Akhfennir',
  ],

  'Dakhla-Oued Ed-Dahab': [
    'Dakhla',
    'Aousserd',
    'Bir Gandouz',
    'El Argoub',
    'Guerguerat',
  ],
};

export const REGIONS = Object.keys(REGION_CITIES);

export function getCitiesForRegion(region: string): string[] {
  return REGION_CITIES[region] ?? [];
}
