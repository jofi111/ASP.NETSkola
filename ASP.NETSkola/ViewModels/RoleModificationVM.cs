﻿namespace ASP.NETSkola.ViewModels
{
    public class RoleModificationVM
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[]? IdsToAdd { get; set; }    
        public string[]? IdsToDelete { get; set; }
    }
}
