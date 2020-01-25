using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2_PuzzleGame
{
    class Utils
    {
        public static bool IsExistElementInArray(List<int> array, int element)
        {

            for(int i = 0; i < array.Count; i++)
            {
                if (element == array[i])
                    return true;
            }

            return false;
        }

        public static List<int> ArrangeRandomArray(List<int> inputList)
        {
            List<int> randomList = new List<int>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }
    }
}
