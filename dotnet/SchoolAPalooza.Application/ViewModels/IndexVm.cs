namespace SchoolAPalooza.Application.ViewModels
{
  public class IndexVm
  {
    public string Hosting { get; private set; }

    public static IndexVm Build(string hosting)
    {
        return new IndexVm
        {
            Hosting = hosting
        };
    }
  }
}
