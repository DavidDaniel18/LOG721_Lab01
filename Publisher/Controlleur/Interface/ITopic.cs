using Configuration.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Collections.Generic;

namespace Controlleur.Interface
{
    interface ITopic
    {
        string getName();
        List<dynamic> getPub();
        List<dynamic> getSub();

    }
}