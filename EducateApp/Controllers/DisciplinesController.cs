using ClosedXML.Excel;
using EducateApp.Models;
using EducateApp.Models.Data;
using EducateApp.ViewModels.Disciplines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EducateApp.Controllers
{
    [Authorize(Roles = "admin, registeredUser")]
    public class DisciplinesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;//созданна переменная для данного контекста**

        public DisciplinesController(AppCtx context,
         UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }
        // GET: Disciplines
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.Disciplines
                  .Include(f => f.User)                // и связываем с таблицей пользователи через класс User
                  .Where(f => f.IdUser == user.Id)     // устанавливается условие с выбором записей форм обучения текущего пользователя по его Id
                  .OrderBy(f => f.IndexProfModule).ThenBy(f => f.Index);          // сортируем все записи по Проф. модуль

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }


        // GET: Disciplines/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDisciplineViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(f => f.IdUser == user.Id &&
                    f.IndexProfModule == model.IndexProfModule && 
                    f.ProfModule == model.ProfModule && 
                    f.Index == model.Index &&
                    f.Name == model.Name &&
                    f.ShortName == model.ShortName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная дисциплина уже существует");
            }

            if (ModelState.IsValid)
            {
                Discipline discipline = new()
                {
                    IndexProfModule = model.IndexProfModule,
                    ProfModule = model.ProfModule,
                    Index = model.Index,
                    Name = model.Name,
                    ShortName = model.ShortName,
                    IdUser = user.Id
                };

                _context.Add(discipline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Disciplines/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines.FindAsync(id);
            if (discipline == null)
            {
                return NotFound();
            }

            EditDisciplineViewModel model = new()
            {
                Id = discipline.Id, 
                IndexProfModule = discipline.IndexProfModule,
                ProfModule = discipline.ProfModule,
                Index = discipline.Index,
                Name = discipline.Name,
                ShortName = discipline.ShortName,
                IdUser = discipline.IdUser
            };

            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditDisciplineViewModel model)
        {
            Discipline discipline = await _context.Disciplines.FindAsync(id);

            if (id != discipline.Id)
            {
                return NotFound();
            }

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Disciplines
                .Where(f => f.IdUser == user.Id &&
                    f.IndexProfModule == model.IndexProfModule &&
                    f.ProfModule == model.ProfModule &&
                    f.Index == model.Index &&
                    f.Name == model.Name &&
                    f.ShortName == model.ShortName).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная дисциплина уже существует");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    discipline.IndexProfModule = model.IndexProfModule;
                    discipline.ProfModule = model.ProfModule;
                    discipline.Index = model.Index;
                    discipline.Name = model.Name;
                    discipline.ShortName = model.ShortName;
                    _context.Update(discipline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplineExists(discipline.Id))
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

        private bool DisciplineExists(object id)
        {
            throw new NotImplementedException();
        }

        // GET: Disciplines/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var discipline = await _context.Disciplines.FindAsync(id);
            _context.Disciplines.Remove(discipline);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Disciplines/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Disciplines
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (discipline == null)
            {
                return NotFound();
            }

            return View(discipline);
        }

        public FileResult DownloadPattern()
        {
            IXLRange rngBorder;     // объект для работы с диапазонами в Excel (выделение групп ячеек)

            // создание книги Excel
            using (XLWorkbook workbook = new(XLEventTracking.Disabled))
            {
                int i = 1;
                // добавить лист в книгу Excel
                IXLWorksheet worksheet = workbook.Worksheets
                    .Add($"Дисциплины");

                // в первой строке текущего листа указываем:
                // заголовки у столбцов
                worksheet.Cell("A" + i).Value = "Индекс проф. модуля";
                worksheet.Cell("B" + i).Value = "Проф. модуль";
                worksheet.Cell("C" + i).Value = "Индекс";
                worksheet.Cell("D" + i).Value = "Дисциплина";
                worksheet.Cell("E" + i).Value = "Краткое название";

                // устанавливаем внешние границы для диапазона A1:F1
                rngBorder = worksheet.Range("A1:E1");       // создание диапазона (выделения ячеек)
                rngBorder.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;       // для диапазона задаем внешнюю границу

                // на листе для столбцов задаем значение ширины по содержимому
                worksheet.Columns().AdjustToContents();

                // создаем стрим
                using (MemoryStream stream = new())
                {
                    // помещаем в стрим созданную книгу
                    workbook.SaveAs(stream);
                    stream.Flush();

                    // возвращаем файл определенного типа
                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"disciplines_{DateTime.UtcNow.ToShortDateString()}.xlsx"     //в названии файла указываем таблицу и текущую дату
                    };
                }
            }
        }

        private bool DisciplineExists(short id)
        {
            return _context.Disciplines.Any(e => e.Id == id);
        }
    }
}
