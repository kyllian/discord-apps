namespace TheFiremind.Models;
record Card(string Name, double Cmc, ImageUris Uri, string OracleText, string RulingsUri, string Mana);
record ImageUris(string Png, string BorderCrop, string Normal);
record Ruling(string Source, DateTime Date, string Comment);
record RulingsList(Ruling[] Rulings);