
//Header's guard

#ifndef string
#include<string>
#endif

#ifndef iostream
#include<iostream>
#endif

#ifndef ctime
#include<ctime>
#endif

using std::string;
using std::cin;
using std::cout;
using std::endl;

//Functions prototypes

void coutFast(string str);

//Anny's system

class Anny{
	
	//Private variables
	
	string name = "";
	string data = "";
	const int CSTTimeZone=4;
	
	public:
		
		//Constructor
		
		Anny(string name){
			this->name = name;
			
			cout<<"\nWelcome to Anny's system, I'm Anny, ask me what ever you want " << name << "!!!";
			cout<<"\n\nType \"exit\" to close the app.\n";
		}
		
		//Asking function
		
		void Ask(string question){
			
			//Entry msg
			
			if(question=="exit")
				return;
			
			cout << "\nResponse: ";
			
			//Msg
			
			if(question == "Hi")
				coutFast("Hi!");
			
			else if(question == "How are you?")
				coutFast("Fine, I hope you're the same!");
			
			else if(question == "What is your favorite movie?")
				coutFast("Shrek!");
			
			else if(question == "What is your favorite color?")
				coutFast("Red!");
			
			else if(question == "What is the time?"){
				cout << "Current time = ";
				cout << ((time (0)/60/60)-CSTTimeZone) %24  << ":" ;  // hours
				cout << (time (0)/60) %60 << ":" ;  // minutes
				cout << (time (0)) %60;  // seconds
			}
			
			else
				coutFast("I'm sorry, I don't get you!");
			
			//endline (Buffer flush)
			
			cout << endl;
		}
		
		//Destructor
		
		~Anny(){
			cout<<"\nGood bye " << name << endl;
		}
};

int main(){
	
	//Read nanme
	
	string data("");
	cout << "\nWrite your name: ";
	
	if(!getline(cin,data) or (end(data)-begin(data) < 1)){
		std::cerr << "\nFatal error!"<<endl;
		return -1;
	}
	
	//Create dynamic pointer
	
	Anny *a = new Anny(data);
	
	//Ask started
	
	do{
		coutFast("\nTalk to me: ");
		getline(cin,data);
		a->Ask(data);
		
	}while(data != "exit");
	
	//Bye msg (Call destructor from class)
	
	delete a;
	
	return 0;
}

//Function definition

void coutFast(string str){
	cout << str;
}
