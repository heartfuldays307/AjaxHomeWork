using AjaxDemo.Models;
using AjaxDemo.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace AjaxDemo.Controllers
{
    public class ApiController : Controller
    {
        private readonly MydbContext _content;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ApiController(MydbContext content, IWebHostEnvironment webHostEnvironment)
        {
            _content = content;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var citys = _content.Addresses.Select(x => x.City).Distinct();
            return Json(citys);
        }

        public IActionResult Avatar(int id = 1)
        {
            var member = _content.Members.Find(id);
            if (member != null)
            {
                byte[] img = member.FileData;
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult Index1(string cities)
        {

            var SiteId = (from c in _content.Addresses
                         where c.City == cities
                         select c.SiteId).Distinct();



            return Json(SiteId);
        }

        public IActionResult Index2(string sites)
        {
            var Road = (from c in _content.Addresses
                          where c.SiteId == sites
                          select c.Road).Distinct();
            return Json(Road);
        }

        public IActionResult Index3()
        {
            string content = "Ajax, 您好";
            return Content(content, "text/plain", System.Text.Encoding.UTF8);
        }

        public IActionResult Register(User _user)
        {
            if (string.IsNullOrEmpty(_user.userName)) 
                _user.userName = "guest";

            else
            {
                var check = (from c in _content.Members
                            where c.Name == _user.userName
                             select c.Name).Distinct().FirstOrDefault();
                if (check == null) 
                    return Content("姓名不存在 帳號可使用", "text/plain");
            }

            string strPath = Path.Combine(_webHostEnvironment.WebRootPath,"images",_user.photo.FileName);

            using (var fileStream = new FileStream(strPath,FileMode.Create))
            {
                _user.photo.CopyTo(fileStream);
            }

            return Content($"{_user.userName}-{_user.userEmail}-{_user.userAge}-{strPath}", "text/plain");
        }
        //寫資料庫
        public IActionResult Register2(User _user)
        {
            Member m = new Member();

            m.Name = _user.userName;
            m.Email = _user.userEmail;
            m.Age = _user.userAge;
            m.FileName = _user.photo.FileName;

            string strPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", _user.photo.FileName);
            using (var fileStream = new FileStream(strPath, FileMode.Create))
            {
                _user.photo.CopyTo(fileStream);
            }

            byte[] imgByte = null;

            using (var memorySteam = new MemoryStream())
            {
                _user.photo.CopyTo(memorySteam);
                imgByte = memorySteam.ToArray();

            }
            m.FileData = imgByte;

            _content.Members.Add(m);
            _content.SaveChanges();

            return Content("成功寫進資料庫");
        }

        public IActionResult Spots([FromBody] SearchDTO searchDTO)
        {
            var spots = searchDTO.categoryId == 0 ? _content.SpotImagesSpots :
                        _content.SpotImagesSpots.Where(s => s.CategoryId == searchDTO.categoryId);

            if (!string.IsNullOrEmpty(searchDTO.keyword))
                spots = spots.Where(s => s.SpotTitle.Contains(searchDTO.keyword) || s.SpotDescription.Contains(searchDTO.keyword));
                

            switch (searchDTO.sortBy)
            {
                case "spotTitle": 
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s=>s.SpotTitle):
                                    spots.OrderByDescending(s=>s.SpotTitle); 
                    break;
                case "categoryId":
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.CategoryId) :
                                    spots.OrderByDescending(s => s.CategoryId);
                    break;
                default:
                    spots = searchDTO.sortType == "asc" ? spots.OrderBy(s => s.SpotId):
                                    spots.OrderByDescending(s => s.SpotId); 
                    break;
            }

            //總共有多少筆資料
            int totalCount = spots.Count();
            int pageSize = searchDTO.pageSize;
            int page = searchDTO.page;
            //計算總共有幾頁
            int totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            spots = spots.Skip((page - 1) * pageSize).Take(pageSize);
            SpotsPagingDTO spotsPaging = new SpotsPagingDTO();

            spotsPaging.categoryName = _content.Categories.Select(s => s.CategoryName).ToList();
            spotsPaging.TotalPages = totalPages;
            spotsPaging.SpotsResult = spots.ToList();

            return Json(spotsPaging);
        }

    }
}
