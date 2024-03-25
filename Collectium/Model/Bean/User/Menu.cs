using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Bean.User
{
    public class Menu
    {

        public Menu()
        {
            this.Children = new List<Menu>();
        }

        public string? Title { get; set; }

        public string? Icon { get; set; }

        public string? Route { get; set; }

        public ICollection<Menu>? Children { get; set; }

        public void AddChildren(Menu child)
        {
            this.Children.Add(child);
        }

    }
}
