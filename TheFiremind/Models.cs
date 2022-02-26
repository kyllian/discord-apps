using Newtonsoft.Json;

namespace TheFiremind;
record Card(string Id, string Name);
record ImageUris(string Png, string BorderCrop, string Normal);
record Ruling(string Source, DateTime Date, string Comment);
record ScryfallSingleObject<T>(T Data);