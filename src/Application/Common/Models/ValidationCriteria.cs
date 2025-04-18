using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomusFix.Api.Application.Common.Models;
public class ValidationCriteria
{
    public List<string> RequiredPhrases { get; set; } = default!;// For "phrase_presence"
    public List<string> SectionKeywords { get; set; } = default!; // For "section_presence"
    public string SectionName { get; set; } = default!;
}
