using OnlineCash.DataBaseModels;
using System;

namespace OnlineCash.Services
{
    public class DocSynchScopeService
    {
        private DocSynch docSynch { get; set; }
        public DocSynch NewDocSynch(Guid uuid)
        {
            docSynch = new DocSynch { Uuid=uuid};
            return docSynch;
        }

        public void SetSynchSuccess(TypeDocs typeDoc, int id, bool isSuccess=true)
        {
            docSynch.TypeDoc = typeDoc;
            docSynch.DocId = id;
            docSynch.isSuccess = isSuccess;
        }
    }
}
