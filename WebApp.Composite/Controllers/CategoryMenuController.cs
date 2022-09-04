using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Composite.Composite;
using WebApp.Composite.Models;

namespace WebApp.Composite.Controllers
{
    [Authorize]
    public class CategoryMenuController : Controller
    {
        private readonly AppIdentityDbContext _context;
        public CategoryMenuController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.First(x=> x.Type == ClaimTypes.NameIdentifier).Value;
            var categories = await _context.Categories.Include(x=> x.Books).Where(m=> m.UserId == userId).OrderBy(x=> x.Id).ToListAsync();
            var menu = GetMenus(categories, new Category { Name = "Top Category", Id = 0 }, new BookComposite(0,"Top Menu"));
            ViewBag.menu = menu;
            ViewBag.selectList = menu.Components.SelectMany(x => ((BookComposite)x).GetSelectListItems(""));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int categoryId, string bookName)
        {
            await _context.Books.AddAsync(new Book { CategoryId = categoryId, Name = bookName });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public BookComposite GetMenus(List<Category> categories,Category topCategory,BookComposite topBookComposite,BookComposite last=null)
        {
            categories.Where(x => x.ReferenceId == topCategory.Id).ToList().ForEach(category =>
            {
                var bookComposite = new BookComposite(category.Id, category.Name);
                category.Books.ToList().ForEach(book =>
                {
                    bookComposite.Add(new BookComponent(book.Id, book.Name));
                });
                if(last != null)
                {
                    last.Add(bookComposite);
                }
                else
                {
                    topBookComposite.Add(bookComposite);
                }
                GetMenus(categories, category, topBookComposite, bookComposite);
            });
            return topBookComposite;
        }
    }
}
