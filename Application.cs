using System;

namespace POE {

    internal class Application {
        //Small enum that tells the app what state to be in to determine what logical operators to perform
        //The value of the enum refers to the maximum options of each state
        //Enum is defined in the class as it local to this class only
        private enum AppState {
            noRecipe = 2,
            hasRecipe = 5
        }
        //Get the state of the application
        private AppState state;
        //The application loop
        private bool appFinished = false;

        public Application() {
            //Showing the application has no recipe therefore no persitance
            this.state = AppState.noRecipe;
            //Shpwing the apploop has continued
            this.appFinished = false;
        }

        public void RunApp() {
            //Main App loop
            do {
                //The users option defined for the do while scope 
                byte option;
                //predefine the validity of the users input defined for the do while scope
                bool optionValid = false;

                do {
                    //Display the menu
                    this.Menu(this.state);
                    //Get the users option
                    option = this.GetBytes();

                    //Determine the validity of the option using a guard claus
                    if (option == 0 || option > (int)this.state) {
                        //Alert the user of thier invalid option
                        Console.Write("Option Invalid!\nPress Any Key to Continue...");
                        Console.ReadKey();
                        //Clear the console for readability purposes
                        Console.Clear();
                        continue;
                    }

                    //state the option is valid as it does not continue through the guard clause statement
                    optionValid = true;
                    //Continue the loop if there is no valid option added
                } while (!optionValid);

                //Handle the users valid input given the current state
                this.HandleEvent(this.state, option);
                //Clear the console for the users use
                Console.Clear();

                //Exit the app if the apploop is finished
            } while (!this.appFinished);
            //Say goodbye to the user as they are finished with the application
            Console.WriteLine("Come Again Soon!");
            //Tell the user how to close
            Console.WriteLine("Press any key to close...");
            //Wait for the user to close the application
            Console.ReadKey();
        }

        private void Menu(AppState state) {
            //set the defualt prompt to an empty string for compiler warnings
            string prompt = string.Empty;

            //Swtich the current state
            switch (state) {
                //Select the prompt if there is no recipe
                case AppState.noRecipe: {
                    //Set the no recipe prompt
                    prompt = "1) Add your recipe\n2) Exit";
                    //Break out the case
                    break;
                }

                //Select the prompt if there is a recipe
                case AppState.hasRecipe: {
                    //Set the has recipe prompt
                    prompt = "1) View recipe\n2) Adjusted recipe quantities\n3) Reset quantities\n4) Clear Data\n5) Exit";
                    //Break out the case statement
                    break;
                }
            }

            //output the appropriate prompt
            Console.WriteLine(prompt);
            //Write a line for the users input
            Console.Write("Enter option: ");
        }

        /// <summary>
        /// This method handles the users enter option
        /// </summary>
        /// <param name="state">The current state of the app</param>
        /// <param name="option">The users selected option</param>
        private void HandleEvent(AppState state, byte option) {
            //Switch on the current state
            switch (state) {
                //Case if there is no recipe
                case AppState.noRecipe: {
                    //Go to the method that handles the events related to entering a recipe
                    this.EnterRecipeEventHandler(option);
                    //Break this current case
                    break;
                }

                //Case if there is a recipe
                case AppState.hasRecipe: {
                    //Go to the method that handles the events related to having a current recipe
                    this.RecipeEventHandler(option);
                    //Break the current case
                    break;
                }
            }
        }

        #region General Methods
        //here are General Functions
        /// <summary>
        /// This method is responsible for getting and validating the users input
        /// </summary>
        /// <returns>0 if there is an exception throw or the value the user entered</returns>
        private byte GetBytes() {
            byte userInput;
            try {
                userInput = Convert.ToByte(Console.ReadLine());
            } catch (OverflowException) {
                return 0;
            } catch (FormatException) {
                return 0;
            }
            return userInput;
        }

        private byte GetBytes(out bool r) {
            r = false;
            string userInput;
            try {
                userInput = Console.ReadLine();
                if (userInput == "r") {
                    r = true;
                }
                return Convert.ToByte(userInput);
            } catch (OverflowException) {
                return 0;
            } catch (FormatException) {
                return 0;
            }
        }

        /// <summary>
        /// This method is used to set the appFinished field to true to exit the apploop
        /// </summary>
        private void Exit() {
            //Sets appfinished to true
            this.appFinished = true;
        }
        #endregion

        #region No Recipe Methods
        //Here are the events of the app without a recipe
        /// <summary>
        /// This method Handles the users option event for when there is no recipe
        /// </summary>
        /// <param name="option">The users option that was chosen</param>
        private void EnterRecipeEventHandler(byte option) {
            switch (option) {
                case (1): {
                    this.EnterRecipe();
                    break;
                }

                case (2): {
                    this.Exit();
                    break;
                }
            }
        }
        /// <summary>
        /// Allows the user to enter the recipe of their choice
        /// </summary>
        private void EnterRecipe() {
            bool restart;
            do {
                //Default the user restart to false
                restart = false;
                //Tell the user what we are doing
                Console.WriteLine("Lets start with the amount of ingredients you want to add!");
                Console.WriteLine("You can always restart by typing 'r'");
                //Ask the user for the number of ingredients that needs to be entered
                Console.Write("Enter the amount of ingredients for your recipe: ");
                //Get the number of ingredients that need to be entered
                byte numIngredients = this.GetBytes();

                //Make an array of Ingredient names unit of measurements and quantities for the users recipe
                string[] ingredientNames = new string[numIngredients];
                UnitOfMeasurement[] UoM = new UnitOfMeasurement[numIngredients];
                byte[] quantity = new byte[numIngredients];

                //Do a for loop for the number of ingredients the user entered
                for (int i = 0; i < numIngredients; i++) {
                    //Get the ingredient name
                    ingredientNames[i] = this.GetIngredientName(out restart);
                    if (restart) {
                        continue;
                    }
                    UoM[i] = this.GetUnitOfMeasurement(out restart);
                    if (restart) {
                        continue;
                    }
                    quantity[i] = this.GetIngredientQuantity(UoM[i], out restart);
                    if (restart) {
                        continue;
                    }
                }
                if (restart) {
                    Console.WriteLine("Lets restart then\nPress any key to continue...");
                    Console.Clear();
                }
            } while (restart);
        }

        private string GetIngredientName(out bool pressedR) {
            pressedR = false;
            Console.Write("Enter ingredient name: ");
            string name = Console.ReadLine();
            if (name.ToUpper() == "R") {
                pressedR = true;
            }
            return name;
        }

        /// <summary>
        /// This method allows for the user to enter a unit of measurement for their ingredient
        /// </summary>
        /// <param name="pressedR">Checks to see if a user pressed R to restart the program</param>
        /// <returns>Unit of measurment</returns>
        private UnitOfMeasurement GetUnitOfMeasurement(out bool pressedR) {
            pressedR = false;
            bool validUoM = false;
            UnitOfMeasurement returns = UnitOfMeasurement.None;
            do {
                Console.WriteLine("Select your unit of measurement");
                Console.Write("1) Tsp\n2) Tbsp\n3) G\n4) KG\n5) Ml\n6) L\nEnter unit here: ");
                string UserInput = Console.ReadLine();
                if (UserInput.ToUpper() == "R") {
                    pressedR = true;
                    return 0;
                }
                byte UoM;
                try {
                    UoM = Convert.ToByte(UserInput);
                    returns = (UnitOfMeasurement)UoM;
                    validUoM = true;
                } catch (OverflowException) {
                    Console.WriteLine("That number is too big!");
                } catch (FormatException) {
                    Console.WriteLine("Enter a valid Number!");
                }
            } while (!validUoM);
            return returns;
        }

        private byte GetIngredientQuantity(UnitOfMeasurement uom, out bool pressedR) {

            Console.Write($"Enter amount of {UoMConversions.UoMToName(uom)}");
            byte qty = this.GetBytes(out pressedR);
            if (pressedR) {
                return 0;
            }
            if (qty == 0) {

            }
            return qty;
        }

        #endregion


        #region Has Recipe Methods
        //Here are the events of the app with a recipe
        /// <summary>
        /// This method handles the events if there is a recipe
        /// </summary>
        /// <param name="option">the users entered option</param>
        private void RecipeEventHandler(byte option) {
            switch (option) {
                case (1): {
                    break;
                }
                case (2): {
                    break;
                }
                case (3): {
                    break;
                }
                case (4): {
                    break;
                }
                case (5): {
                    this.Exit();
                    break;
                }
            }
        }
        #endregion
    }
}