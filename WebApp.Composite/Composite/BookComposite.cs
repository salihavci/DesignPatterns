using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;

namespace WebApp.Composite.Composite
{
    public class BookComposite : IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private List<IComponent> _components;

        public BookComposite(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
            _components = new List<IComponent>();
        }
        public void Add(IComponent component)
        {
            _components.Add(component);
        }
        public void Remove(IComponent component)
        {
            _components.Remove(component);
        }
        public int Count()
        {
            return _components.Sum(x => x.Count());
        }

        public string Display()
        {
            var sb = new StringBuilder();
            sb.Append($"<div class='text-primary my-1'><a href='#' class='menu'>{this.Name} ({Count()})</a></div>");
            
            if (!_components.Any()) return sb.ToString();

            sb.Append("<ul class='list-group list-group-flush ml-3'>");

            foreach (var item in _components)
            {
                sb.Append(item.Display());
            }

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}
