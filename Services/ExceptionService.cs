namespace Shops.ShopServices;
public Task WriteNewCsvFile(string path, string errors, string data)
{
    using (StreamWriter writer = new StreamWriter(path))
    {
        writer.WriteLine(errors);
    }
}
