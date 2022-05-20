#include<iostream>

//ADB Command helper by Starlyn1232 (C++)

using namespace std;

int main(int argc, char *argv[])
{	
	//Error for curious people (Like me :p)
	
	if(argc < 3)
	{
		cerr << "\nStarlyn: What?!";
	    return -1;
	}
	
	//Our system cmd variable    
	
	char adbCmd[350] = "";
	adbCmd[0] = '\"';
	
	bool foundADB = false;
	
	int i[2] = {0,(argc-1)};
	
	//Check adb.exe as argument
	
	for(i[0]=0;i[0]<argc;i[0]++)
	{
		//Break loop flow, and check found as true	
	
		if(strstr(argv[i[0]],"adb.exe"))
		{
			foundADB = true;
			break;
		}
	}
		
	if(!foundADB)
	{
		cerr << "\nADB.EXE WASN'T FOUND! (v1, please try again!)\a";
		return -1;	
	}
	
	//Check adb.exe as argument, but at the correct syntax index
			
	if(strstr (argv[argc-1],"adb.exe"))
	{
		cerr << "\nADB.EXE WASN'T FOUND! (v2, actually was, but there is an error at syntax!!!)\a";
		return -1;
	}
		
	//Directory with whitespaces? Don't worry fulk!
			
	for(i[0] = 1;i[0]<=i[1];i[0]++)
	{		
		strcat(adbCmd,argv[i[0]]);
		
		if(strstr (argv[i[0]],"adb.exe"))
			strcat(adbCmd,"\"");
						
		strcat(adbCmd," ");
	}
	
	//Run the system cmd
	
	//cout << "\nRunning command: \n\n" << adbCmd << "\n\n" << endl;
	system(adbCmd);
	
	return 0;
}