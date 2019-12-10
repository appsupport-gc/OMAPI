using OMAPI.Client.Blazor.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMAPI.Client.Blazor.Shared.Components
{
    public class AdmFieldIDSelectByDropDownModel : ComponentBase
    {
        RESTAdmField[] AFs;
        protected RESTAdmField[] adm1AFs = new RESTAdmField[] { };
        protected RESTAdmField[] adm2AFs = new RESTAdmField[] { };
        protected RESTAdmField[] adm3AFs = new RESTAdmField[] { };

        protected RESTAdmField selectedAF = new RESTAdmField();

        [Inject] IOMDataService OMDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AFs = await OMDataService.FetchRESTAdmFields();
            if (AFs != null) adm1AFs = AFs.Where(a => a.YBOrgLevel <= 1).ToArray();
        }

        [Parameter] public string AdmFieldID { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (AFs == null) await OnInitializedAsync();
            selectedAF = AFs.Where(a => a.AdmFieldID == AdmFieldID).SingleOrDefault();
            if (selectedAF == null)
            {
                AdmFieldID = null;
                selectedAF = new RESTAdmField();
                adm2AFs = new RESTAdmField[] { };
                adm3AFs = new RESTAdmField[] { };
            }
            else
            {
                //if (selectedAF.Adm2 != null)
                adm2AFs = AFs.Where(a => a.ParentID == selectedAF.Adm1 && selectedAF.Adm1 != "GC").ToArray();
                //if (selectedAF.Adm2 != null)
                adm3AFs = AFs.Where(a => a.ParentID == selectedAF.Adm2 && selectedAF.Adm2 != null).ToArray();

            }
        }

        protected void OnAFChange(ChangeEventArgs e)
        {
            if (e.Value.ToString().StartsWith("["))
                AdmFieldID = null;
            else
                AdmFieldID = e.Value.ToString();
            OnParametersSetAsync();
        }
    }
}
