using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryOrderManger.Database;
using InventoryOrderManger.Models;

namespace InventoryOrderManger.Common
{
    public static class SequenceGenerator
    {
        private static DbConnection dbConnection = DbConnection.GetDbConnection();

        public static async Task<string> GetSequenceNo(string sequenceType)
        {
            List<Sequence> sequences = await dbConnection.GetSequences();

            Sequence sequence = sequences.Where(x => x.SequenceType == sequenceType).FirstOrDefault();

            if (sequence == null)
            {
                sequence = new Sequence()
                {
                    SequenceType = sequenceType,
                    Count = 1
                };

                await dbConnection.InsertRecord(sequence);
            }
            else
            {
                sequence.Count++;
                await dbConnection.UpdateRecord(sequence);
            }

            return $"{sequence.SequenceType}-{sequence.Count}({DateTime.Now:dd/MM/yyyy})";
        }
    }
}