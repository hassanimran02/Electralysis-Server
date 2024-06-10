using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pleromi.DAL.Entities
{
    public class Faq
    {
        public int FaqID { get; set; }
        public string? QuestionEn { get; set; }
        public string? AnswerEn { get; set; }
        public string? QuestionAr { get; set; }
        public string? AnswerAr { get; set; }
        public string? QuestionUr { get; set; }
        public string? AnswerUr { get; set; }
        public bool IsActive { get; set; }
    }
}
