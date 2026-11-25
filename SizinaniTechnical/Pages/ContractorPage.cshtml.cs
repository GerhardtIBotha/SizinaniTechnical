using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using SizinaniTechnical.Data;
using SizinaniTechnical.Model;

namespace SizinaniTechnical.Pages
{
    public class ContractorPage : PageModel
    {

        private readonly ContractorRepository _contractorRepo;

        public ContractorPage(ContractorRepository contractorRepository)
        {
            _contractorRepo = contractorRepository;
        }

        [BindProperty]
        public Contractor Input { get; set; } = new();

        [TempData]
        public string? Message { get; set; }

        public List<RegisteredVehicle> Rows { get; set; }
        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                 _contractorRepo.SaveContractor(Input);
                Message = "Added Contractor";
                return RedirectToPage();

            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, $"Database error: {ex.Message}");
                return Page();
            }
        }
    }
}
