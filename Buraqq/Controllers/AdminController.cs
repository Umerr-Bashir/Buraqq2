using Buraqq.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Buraqq.Models.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Buraqq.Controllers
{
    [AuthorizeAdmin]
    public class AdminController : Controller
    {
        private readonly BuraqqContext _context;

        public AdminController(BuraqqContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Index()
        {
            return View(await _context.TeacherDetails.Include(model => model.Teacher).Select(x=> new TeacherInfoViewModel {Id=x.Id,Image=x.Image,Description=x.Description,Email = x.Teacher.Email,Fee = x.Fee,Name=$"{x.Teacher.FirstName} {x.Teacher.LastName}",Subject= _context.TeacherSubjects.Include(y=>y.Subject).Where(y=>y.TeacherId==x.TeacherId).Select(y=>y.Subject.Name).ToList() }).ToListAsync()); ;
        }


        
        public IActionResult TeacherProfileDelete(int id)
        {
            TeacherDetail teacherdetail = _context.TeacherDetails.Where(x => x.Id == id).FirstOrDefault();
            _context.TeacherDetails.Remove(teacherdetail);
            _context.SaveChanges();
            return Redirect("/Admin/Index");
        }



        [HttpGet]
        public async Task<IActionResult> TeacherProfileAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TeacherProfileAdd(TeacherDetail teacherDetail)
        {
            _context.TeacherDetails.Add(teacherDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("/Admin/Index");

        }


        [HttpGet]
        public async Task<IActionResult> TeacherProfileAddEdit(int? id)
        {
            ViewBag.Teachers = new SelectList(_context.Users.Where(x => x.RoleId == 2), "Id", "FirstName");
            if (id == null)
                return View();
            else
                return View(await _context.TeacherDetails.Where(d => d.Id == id).Include(model=>model.Teacher).FirstOrDefaultAsync());

        }
        [HttpPost]
        public async Task<IActionResult> TeacherProfileAddEdit(TeacherDetail teacherDetail)
        {
            _context.TeacherDetails.Update(teacherDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
