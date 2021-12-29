using EducateApp.Models;
using EducateApp.Models.Data;
using EducateApp.ViewModels.TypesOfTotals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class TypesOfTotalsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public TypesOfTotalsController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: TypesOfTotals
        public async Task<IActionResult> Index(TypeofTotalSortState sortOrder = TypeofTotalSortState.CertificateNameAsc)
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            // через контекст данных получаем доступ к таблице базы данных TypesofTotals

            var typesOfTotal = _context.TypesOfTotals
                  .Include(f => f.User)                                           // и связываем с таблицей пользователи через класс User
                  .Where(f => f.IdUser == user.Id);                                // устанавливается условие с выбором записей форм обучения текущего пользователя по его Id
                 ViewData["CertificateNameSort"] = sortOrder == TypeofTotalSortState.CertificateNameAsc ? TypeofTotalSortState.CertificateNameDesc : TypeofTotalSortState.CertificateNameAsc;

            typesOfTotal = sortOrder switch
            {
                TypeofTotalSortState.CertificateNameDesc => typesOfTotal.OrderByDescending(s => s.CertificateName),
                _ => typesOfTotal.OrderBy(s => s.CertificateName),
            };

            // возвращаем в представление полученный список записей
            return View(await typesOfTotal.AsNoTracking().ToListAsync());
        }

        // GET: TypesOfTotals/Details/5
       
        // GET: TypesOfTotals/Create
        public IActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTypeOfTotalViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.TypesOfTotals
                .Where(f => f.IdUser == user.Id &&
                    f.CertificateName == model.CertificateName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеный тип промежуточной аттестации уже существует");
            }

            if (ModelState.IsValid)
            {
                TypeofTotal typeofTotal = new()
                {
                    CertificateName = model.CertificateName,
                    IdUser = user.Id
                };

                _context.Add(typeofTotal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: TypesOfTotals/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeofTotal = await _context.TypesOfTotals.FindAsync(id);
            if (typeofTotal == null)
            {
                return NotFound();
            }

            EditTypeOfTotalViewModel model = new()
            {
                Id = typeofTotal.Id,
                CertificateName = typeofTotal.CertificateName,
                IdUser = typeofTotal.IdUser
            };

            return View(typeofTotal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditTypeOfTotalViewModel model)
        {
            TypeofTotal typeofTotal = await _context.TypesOfTotals.FindAsync(id);
            if (id != typeofTotal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    typeofTotal.CertificateName = model.CertificateName;
                    _context.Update(typeofTotal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeofTotalExists(typeofTotal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: TypesOfTotals/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeofTotal = await _context.TypesOfTotals
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeofTotal == null)
            {
                return NotFound();
            }

            return View(typeofTotal);
        }

        // POST: TypesOfTotals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var typeofTotal = await _context.TypesOfTotals.FindAsync(id);
            _context.TypesOfTotals.Remove(typeofTotal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeofTotal = await _context.TypesOfTotals
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (typeofTotal == null)
            {
                return NotFound();
            }

            return PartialView(typeofTotal);
        }

        private bool TypeofTotalExists(short id)
        {
            return _context.TypesOfTotals.Any(e => e.Id == id);
        }


    }
}
