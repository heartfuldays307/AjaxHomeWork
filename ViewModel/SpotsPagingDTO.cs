using AjaxDemo.Models;
using System.Composition.Convention;

namespace AjaxDemo.ViewModel
{
    public class SpotsPagingDTO
    {
        public int TotalPages { get; set; }

        public List<string>? categoryName { get; set; }
        public List<SpotImagesSpot>? SpotsResult { get; set; }
    }
}
