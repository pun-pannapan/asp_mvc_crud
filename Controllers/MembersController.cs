using asp_mvc_crud.Models;
using asp_mvc_crud.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace asp_mvc_crud.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllAsync();
            return View(members);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var member = await _memberService.GetByIdAsync(id.Value);
            if (member == null) return NotFound();

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Active" },
                new { Value = "Inactive", Text = "Inactive" },
                new { Value = "Suspended", Text = "Suspended" }
            }, "Value", "Text");

            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,Status")] Member member)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (await _memberService.IsEmailExistsAsync(member.Email))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }
                else
                {
                    await _memberService.CreateAsync(member);
                    TempData["Success"] = "Member created successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Active" },
                new { Value = "Inactive", Text = "Inactive" },
                new { Value = "Suspended", Text = "Suspended" }
            }, "Value", "Text", member.Status);

            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var member = await _memberService.GetByIdAsync(id.Value);
            if (member == null) return NotFound();

            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Active" },
                new { Value = "Inactive", Text = "Inactive" },
                new { Value = "Suspended", Text = "Suspended" }
            }, "Value", "Text", member.Status);

            return View(member);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,FirstName,LastName,Email,Phone,JoinDate,Status")] Member member)
        {
            if (id != member.MemberId) return NotFound();

            if (ModelState.IsValid)
            {
                // Check if email already exists for other members
                if (await _memberService.IsEmailExistsAsync(member.Email, member.MemberId))
                {
                    ModelState.AddModelError("Email", "Email already exists");
                }
                else
                {
                    await _memberService.UpdateAsync(member);
                    TempData["Success"] = "Member updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.StatusList = new SelectList(new[]
            {
                new { Value = "Active", Text = "Active" },
                new { Value = "Inactive", Text = "Inactive" },
                new { Value = "Suspended", Text = "Suspended" }
            }, "Value", "Text", member.Status);

            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var member = await _memberService.GetByIdAsync(id.Value);
            if (member == null) return NotFound();

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _memberService.DeleteAsync(id);
            if (result)
            {
                TempData["Success"] = "Member deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to delete member!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
