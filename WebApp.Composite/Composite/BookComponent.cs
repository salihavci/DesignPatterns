namespace WebApp.Composite.Composite
{
    public class BookComponent : IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BookComponent(int Id, string Name)
        {
            this.Name = Name;
            this.Id = Id;
        }

        public int Count()
        {
            return 1;
        }

        public string Display()
        {
            return $"<li class='list-group-item'>{this.Name}</li>";
        }
    }
}
