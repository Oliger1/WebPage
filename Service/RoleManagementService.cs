using Microsoft.AspNetCore.Identity;

public class RoleManagementService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleManagementService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task CreateRolesAndAssignToUserAsync()
    {
        // Krijimi i roleve
        await _roleManager.CreateAsync(new IdentityRole("Admin"));
        await _roleManager.CreateAsync(new IdentityRole("User"));

        // Lidhja e përdoruesve me rolet
        var user = await _userManager.FindByEmailAsync("example@example.com");
        await _userManager.AddToRoleAsync(user, "Admin");
        await _userManager.AddToRoleAsync(user, "User");
    }
}
