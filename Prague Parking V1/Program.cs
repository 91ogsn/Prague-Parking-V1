






using System.Transactions;

string[] parkingGarage = new string[100];
string skiljetecken1 = "#";
string skiljetecken2 = "|";
string fordonstyp;
string regNr;

//Test värden
parkingGarage[0] = "CAR#ABC123";
parkingGarage[1] = "MC#AAA333";
parkingGarage[2] = "MC#BBB444|MC#CCC555";

bool exitProgram = false;

MainMenu();

// ------ Metoder ------ \\

// Metod för huvudmeny
void MainMenu()
{
    while (exitProgram == false)
    {
        PrintMenu();

        ConsoleKey menuChoice = Console.ReadKey().Key;

        switch (menuChoice)
        {
            // [1] Park a vehicle
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:


                fordonstyp = GetFordonsTyp();
                regNr = GetRegNr();
                int parkingNr = GetFirstFreePSpace(parkingGarage, fordonstyp);

                //-99 betyder full parkering
                if (parkingNr == -99)
                {
                    Console.WriteLine("Parking garage is full! (Press any key to return to Menu)");
                    Console.ReadKey();
                    break;
                }
                else
                {

                    ParkVehicle(parkingGarage, fordonstyp, skiljetecken1, regNr, parkingNr);
                    PrintArbetsorder(fordonstyp, regNr, parkingNr);

                }

                break;
            // [2] Move a vehicle manually
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                // Call method to move a vehicle manually
                //Console.WriteLine(""); by reg nr or by index
                //int indexFrom;
                //int indexTo;
                break;
            // [3] Check out a vehicle
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                
                PrintHeader();
                Console.Write("\n\tEnter registration number of vehicle to check out: ");
                string checkOutRegNr = Console.ReadLine().ToUpper();
                int checkOutIndex = SearchForVehicle(parkingGarage, checkOutRegNr);
                CheckOutVehicle(parkingGarage, checkOutIndex, checkOutRegNr);

                // Arbetsorder hämta bil från plats ## +1
                Console.WriteLine("\t\tGet vehicle: {0} at parkingspace: {1}", checkOutRegNr, checkOutIndex + 1);
                Console.WriteLine("\n\n\t\tPress any key to return to menu...");
                Console.ReadKey();
                break;

            // [4] Search for a vehicle
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                
                PrintHeader();
                Console.Write("\n\t\tEnter registration number to search for: ");
                string targetRegNr = Console.ReadLine().ToUpper();

                int indexPPlats = SearchForVehicle(parkingGarage, targetRegNr);

                if (indexPPlats != -99)
                {
                    Console.WriteLine("\t\tVehicle was found!\n");
                    Console.WriteLine(HämtaPRuta(parkingGarage, indexPPlats));
                    Console.WriteLine("\n\n\t\tPress any key to return to menu..");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("The given registrationnumber does not exist in the Parking garage.");
                    Console.ReadKey();
                }
                break;

            // [5] Exit the program fråga om användaren är säker
            case ConsoleKey.D5:
            case ConsoleKey.NumPad5:
                
                PrintHeader();
                Console.Write("\n\t\t\tAre you sure you want to exit? (Y/N)");
                ConsoleKey exitChoice = Console.ReadKey().Key;

                if (exitChoice == ConsoleKey.Y)
                {
                    Console.Clear();
                    Console.WriteLine("Thanks for using Prague Parking V1, Good Bye! (Press any key to exit..)");
                    Console.ReadKey();
                    exitProgram = true;
                }

                break;
            default:
                // Felaktig input
                Console.Beep();

                break;
        }
    }
}

void CheckOutVehicle(string[] parkingGarage, int checkOutIndex, string checkOutRegNr)
{
    if (parkingGarage[checkOutIndex].Contains('|'))
    {
        // Splittar dom två sparade fordonen
        string[] temp = parkingGarage[checkOutIndex].Split('|');

        //kollar om temp[0] innehåller reg nr som ska tas bort, i så fall ta värdet från temp[1]
        if (temp[0].Contains(checkOutRegNr))
        {
            parkingGarage[checkOutIndex] = temp[1];
        }
        else
        {
            parkingGarage[checkOutIndex] = temp[0];
        }

    }
    else
    {
        parkingGarage[checkOutIndex] = "";
    }
}

string HämtaPRuta(string[] parkingGarage, int indexPPlats)
{
    //Fångar ifall det står 2st MC och splittar dom 2 ggr
    if (parkingGarage[indexPPlats].Contains('|'))
    {
        string[] temp2 = parkingGarage[indexPPlats].Split('|');
        string[] tempA = temp2[0].Split('#');
        string[] tempB = temp2[1].Split('#');
        //
        return String.Format("Parkingspace Nr: {0} = |Vehicletype: {1}, RegNr: {2}| and |Vehicletype: {3}, RegNr: {4}|"
            , indexPPlats + 1, tempA[0], tempA[1], tempB[0], tempB[1]);
    }
    else
    {
        string[] temp = parkingGarage[indexPPlats].Split('#');
        return String.Format("\t\tParkingspace Nr: {0}, Vehicletype: {1}, RegNr: {2}",
            (indexPPlats + 1), temp[0], temp[1]);
    }
}


int SearchForVehicle(string[] parkingGarage, string target)
{
    int indexOfTarget = -99;
    for (int i = 0; i < parkingGarage.Length; i++)
    {

        if (parkingGarage[i] != null && parkingGarage[i].Contains(target))
        {
            indexOfTarget = i; break;
        }


    }
    return indexOfTarget;
}

//Skriver ut arbetsorder
void PrintArbetsorder(string fordonstyp, string regNr, int parkingNr)
{
    PrintHeader();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n\t\t\tPark {0}: {1} at parking space: {2}", fordonstyp, regNr, parkingNr + 1);
    Console.ResetColor();
    Console.WriteLine("\n\n\t\t\t(Press any key to return to menu...)");
    Console.ReadKey();
}


// Parkerar fordonet
void ParkVehicle(string[] parkingGarage, string fordonstyp, string skiljetecken1, string regNr, int parkingNr)
{
    if (fordonstyp == "CAR")
    {
        parkingGarage[parkingNr] = fordonstyp + skiljetecken1 + regNr;
    }
    else 
    {
        //Om parkeringsplatsen är helt tom
        if (parkingGarage[parkingNr] == null || parkingGarage[parkingNr] == "")
        {
            parkingGarage[parkingNr] = fordonstyp + skiljetecken1 + regNr;
        }
        //Om parkeringsplatsen innehåller 1 MC lägg till |
        else
        {
            parkingGarage[parkingNr] += skiljetecken2 + fordonstyp + skiljetecken1 + regNr;
        }

    }
    
}

//Kolla första lediga P-plats
int GetFirstFreePSpace(string[] parkingGarage, string fordonstyp)
{
    //Använder värdet -99 för att visa att parkingGarage är fullt
    int freeIndex = -99;
    if (fordonstyp == "CAR")
    {
        
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            if (parkingGarage[i] == "" || parkingGarage[i] == null)
            {
                freeIndex = i;
                break;

            }

        }
    }
    else 
    {
        for (int i = 0; i < parkingGarage.Length; i++)
        {
            //kollar om Parkering med index i innehåller bara en MC
            if (parkingGarage[i].Contains("MC") && parkingGarage[i].Contains('|') == false)
            {
                freeIndex = i;
                break;
            }
            if (parkingGarage[i] == "" || parkingGarage[i] == null)
            {
                freeIndex = i;
                break;

            }
        }
    }
    return freeIndex;
}

string GetRegNr()
{
    Console.Clear();
    PrintHeader();
    Console.Write("\n\t\tEnter vehicle registration number (No blankspaces): ");
    regNr = Console.ReadLine().ToUpper();
    Console.Clear();

    return regNr;
}

string GetFordonsTyp()
{

    while (true)
    {
        Console.Clear();
        PrintHeader();
        Console.Write("\n\t\t\tEnter [C] for Car or [M] for Motorcycle: ");
        ConsoleKey carOrMc = Console.ReadKey().Key;
        switch (carOrMc)
        {
            case ConsoleKey.C:
                fordonstyp = "CAR";
                return fordonstyp;

            case ConsoleKey.M:
                fordonstyp = "MC";
                return fordonstyp;

            default:
                {

                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("\n\t\tWrong input! Press C or M! (Press any key to try again..)");
                    Console.ReadKey();
                    Console.ResetColor();
                    Console.Clear();
                    break;
                }
        }
    }
}

// Metod för att skriva ut menyn
void PrintMenu()
{

    PrintHeader();
    Console.WriteLine("\n\t\t\t(Enter a number 1-5 to navigate menu)\n");
    Console.WriteLine("\t\t\t[1]  Park a vehicle");
    Console.WriteLine("\t\t\t[2]  Move a vehicle manually to another parking spot");
    Console.WriteLine("\t\t\t[3]  Check out vehicle");
    Console.WriteLine("\t\t\t[4]  Search for a vehicle");
    Console.WriteLine("\t\t\t[5]  Exit");
    Console.WriteLine("");
}
void PrintHeader()
{
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write(@"
  _____                              _____           _    _              __      ____ 
 |  __ \                            |  __ \         | |  (_)             \ \    / /_ |
 | |__) | __ __ _  __ _ _   _  ___  | |__) |_ _ _ __| | ___ _ __   __ _   \ \  / / | |
 |  ___/ '__/ _` |/ _` | | | |/ _ \ |  ___/ _` | '__| |/ / | '_ \ / _` |   \ \/ /  | |
 | |   | | | (_| | (_| | |_| |  __/ | |  | (_| | |  |   <| | | | | (_| |    \  /   | |
 |_|   |_|  \__,_|\__, |\__,_|\___| |_|   \__,_|_|  |_|\_\_|_| |_|\__, |     \(_)  |_|
                   __/ |                                           __/ |              
                  |___/                                           |___/               
--------------------------------------------------------------------------------------");
    Console.ResetColor();

}

