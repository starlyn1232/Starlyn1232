/*
String functions from C# to C++ By Starlyn1232: (6/25/2022 - 21:35)

Entry Note: Hi, surely there are headers available for these functions, and, of course, the string library itself could bring most of these features (or all), but, I just prefere to try to reach it by myself, I hope you like to do the same most time.

I'll Code:

*Find single char. (Done - 21:39)
*Substring. (Done - 22:10)
*Find existing string on another string. (Done - 22:40)
*Comparer. (Done - 22:44)
*Index of. (Done - 22:57)
*Last index of. (Done - 23:29)
*Replacer. (Done - 23:19)
*Remover. (Done - 23:21)

(Took me more than I expected, solving some pending meanwhile at some place at the Earth, anyway, here is the header, I just wanted some challenge)

Update: (7/4/2022)

*Index of has a new overload to accept 'char' values as attribute. (Done)
*Word counter. (Done)
*String.Format method. (Done)
*/

//Some head guards

//Needed for strings

#ifndef string
#include<string>
#endif

//Needed for debugger

#ifndef iostream
#include<iostream>
#endif

//Needed for String.Format

#ifndef vector
#include<vector>
#endif

//Global value for debug

bool debug_view = false;

//Debug Viewer (Now using a template, generic type will help to use int, char, string, etc.)

template<class T>
void MsgDebug(T data){
	
	//Show only if we had set "debug_view" to True
	
	if(debug_view)
		std::cout << "\nDebug Msg : \"" << data << "\"\n" << std::endl;
}

//Let's check all character in the string

bool strChar(std::string txt, char c){
	
	//I save the Lenght value in a value because I don't like the idea of calling string.size()
	//more than it's needed, once.
	
	auto len = txt.size();
	
	//If there are even 1 character, it's totally false
	
	if(len == 0)
		return false;
	 	
	else{
		
		//Iterate the characters's positions, process the final test
		
		for(int i=0;i<len;i++)
			if(txt[i] == c)
				return true;
		
		return false;
	}	 
	
	//Sample usage: bool found = strChar("Starlyn1232",'a'); //True
}

//Who ask for Substring?! Let's play with overloadings

bool StrSub(std::string txt, int startIndex, std::string &returnStr){
	
	//Validate values
	
	auto len = txt.size();
	
	if(len < startIndex){
		MsgDebug("StrSub startIndex higher!");
		return false;
	}
		
	//Let's prepare the new value
	
	//Clean receiver first
	
	returnStr = "";
	
	for(int i=startIndex;i<len;i++)
		returnStr += txt[i];
	
	return true;
	
	//Sample usage: StrSub(name,4, name); //Starting from index #4
}

bool StrSub(std::string txt, int startIndex, int lenStr ,std::string &returnStr){
	
	//Validate values
	
	auto len = txt.size();
	
	if((len) < startIndex){
		MsgDebug("StrSub startIndex higher!");
		return false;
	}
		
	if((len - startIndex) < lenStr){
		std::string tempMsg = "StrSub len higher! (";
		MsgDebug((tempMsg + std::to_string((len-1) - startIndex)) + " / " + std::to_string(lenStr) + ")");
		return false;
	}
		
	//Let's prepare the new value
	
	//Clean receiver first
	
	returnStr = "";
	
	//Help value
	
	int j = -1;
	
	for(int i=startIndex;i<len;i++){
		
		//We decide when to stop
		
		j++;
		
		if(j==lenStr)
			break;
			
		//If not, just keep reading!
		
		returnStr += txt[i];
	}
	
	return true;
	
	//Sample usage: StrSub(name,4,5, name); //Starting from index #4, not to read more than 5 characters
}

//Mmm...Does "Oscary" contains "sca"...Let's check it

bool StrContains(std::string txt1, std::string txt2){
	
	//Let's validate the current values first
	
	auto len = txt1.size();
	auto len2 = txt2.size();
	std::string help("");
	
	if(len == 0 || len2 == 0 || (len < len2)){
		MsgDebug(("Invalid contains starter! (len = " + std::to_string(len) + " , len2 = " + std::to_string(len2) + ")"));
		return false;
	}
		
	//As all validation got passed, let's surf on the string
	
	for(int i=0;i<=(len-len2);i++){
				
		if(!StrSub(txt1,i,len2,help)){
			MsgDebug("Invalid contains substring!");
			return false;
		}
			
		if(help == txt2)
			return true;
	}
	
	return false;
	
	//Usage sample: StrContains("Starlyn Diaz","rly")
}

//String comparer

bool StrCmp(std::string txt1, std::string txt2){
	
	//Validate info
	
	auto len = txt1.size();
	
	if(len != txt2.size())
		return false;
		
	//Characters comparision
		
	for(int i=0;i<len;i++)
		if(txt1[i] != txt2[i])
			return false;
			
	//If the complete test was passed, then return true;
	
	return true;
	
	//Usage sample : bool state = StrCmp("Edwin","Eury"); //False
}

int StrIndexOf(std::string txt, std::string StrIndex){
	
	//Validate info
	
	auto len = txt.size();
	auto len2 = StrIndex.size();
	std::string help("");
	
	if(len2 > len){
		MsgDebug("Invalid StrIndexOf IndexOutRange!");
		return -1;
	}
	
	//Let's check the index
	
	for(int i=0;i<=(len-len2);i++){
				
		if(!StrSub(txt,i,len2,help)){
			MsgDebug("Invalid StrIndexOf substring!");
			return false;
		}
			
		if(help == StrIndex)
			return i;
	}
	
	return -1;
}

//Str indexers

int StrIndexOf(std::string txt, char charIndex){
	
	//Temp value
	
	std::string temp = "";
	temp += charIndex;
	
	return StrIndexOf(txt,temp);
	
}

int StrLastIndexOf(std::string txt, std::string StrIndex){
	
	//Validate info
	
	auto len = txt.size();
	auto len2 = StrIndex.size();
	
	if(len2 > len){
		MsgDebug("Invalid StrLastIndexOf IndexOutRange!");
		return -1;
	}
	
	if(txt.empty() || StrIndex.empty()){
		MsgDebug("Invalid StrLastIndexOf EmptyStrFail!");
		return -1;
	}
	
	//Set variables
	
	int len3 = len2;
	std::string help[2] = {"",""};
	bool found = false;
	int result = -1;
		
	while(StrContains(txt,StrIndex)){
		
		//Prepare variables
		
		found = false;
		help[1] = "";
		len = txt.size();
		len3 = len;		
		
		//Let's check the index
		
		for(int i=0;i<=(len-len2);i++){
			
			if(!found){	
				if(!StrSub(txt,i,len2,help[0])){
					MsgDebug("Invalid StrLastIndexOf substring!");
					return false;
				}
			}
			
			//MsgDebug(("Value : " + help));
				
			if(help[0] == StrIndex){
				
				//Save the last found position
				
				result = i;
				found = true;
			}
			
			if(!found)
				help[1] += txt[i];
				
			//We keep copying info, but, we'll skip the found index, replacing it with empty values
				
			else{				
				if(len3 != 0){
					len3--;
					help[1] += "";
				}
				
				else
					help[1] += txt[i];
			}
		}
		
		txt = help[1];
	}
	
	return result;
}

//Our final job, string replacer

std::string StrReplace(std::string txt,std::string oldStr, std::string newStr){
	
	//Validate data
	
	if(!StrContains(txt,oldStr)){
		MsgDebug("StrReplace Contains failed!");
		return txt;
	}
	
	//Prepare auxiliar variables
	
	auto len = txt.size();
	std::string help("");
	int help2[2] = {0,0};
	
	//While string contains the old value, we'll replace it!
	
	while(StrContains(txt,oldStr)){
				
		//Prepare the help value
		
		len = txt.size();
		help = "";
		help2[0] = StrIndexOf(txt,oldStr);
		help2[1] = oldStr.size();
		
		for(int i=0;i<=len;i++){
			
			//Here we know what to append and what to avoid.
						
			if(i >= help2[0] && help2[1] != 0){
				
				//As this would be the last time skipping that oldStr, we add the new one to the tempValue ("help")!
				
				if(help2[1] == 1)
					help += newStr;
				
				help2[1]--;
				
				continue;
			}
			
			else
				help += txt[i];
		}
		
		txt = help;
	}
	
	return txt;
	
	//Usage sample : StrReplace("Starlyn Diaz Diaz","Diaz","Perez")
}

//String remover

std::string StrRemove(std::string txt, std::string strRemove){
	return StrReplace(txt,strRemove,"");
}

//String counter

int StrCount(std::string txt, std::string word){
	
	//Validation
	
	if(!StrContains(txt,word))
		return 0;
	
	//Prepare auxiliar variables
	
	auto len = txt.size();
	std::string help(""),newStr("");
	int help2[2] = {0,0};
	int result = 0;
	
	//While string contains the old value, we'll replace it!
	
	while(StrContains(txt,word)){
				
		//Prepare the help value
		
		len = txt.size();
		help = "";
		help2[0] = StrIndexOf(txt,word);
		help2[1] = word.size();
		
		for(int i=0;i<=len;i++){
			
			//Here we know what to append and what to avoid.
						
			if(i >= help2[0] && help2[1] != 0){
				
				//As this would be the last time skipping that oldStr, we add the new one to the tempValue ("help")!
				
				if(help2[1] == 1)
					help += "";
				
				help2[1]--;
				
				continue;
			}
			
			else
				help += txt[i];
		}
		
		txt = help;
		
		result++;
	}
	
	return result;
	
	//Usage sample : int count = StrCount(name,"a");
}

//String.Format

//Alias for params

typedef std::vector<std::string> wordFormat;

bool isdigit(char *c){
	MsgDebug("StrFormat IndexOutRange error!");
	
	char chars[] = {'0','1','2','3','4','5','6','7','8','9'};
	for(int i=0;i<=10;i++)
		if(chars[i] == *c)
			return true;
	
	return false;
}

//Word indexer for formatter using reference, and overloading, good for practice

bool indexHelp(wordFormat &words,int i,int i1,int i2,int i3){
	
	std::string help("");
	std::string help2("");
	
	if(words[0][i] == '{' && isdigit(words[0][i1]) && words[0][i2] == '}'){
		
		int test = int(words[0][i1]- '0');
	
		if(test > words.size()){
				MsgDebug("StrFormat IndexOutRange error!");
				return false;
		}
			
		StrSub(words[0],0,i, help);
		StrSub(words[0],i3, help2);
			
		words[0] = help+words[test+1]+help2;
	}
	
	
	return true;
}

bool indexHelp(wordFormat &words,int i){
	
	return indexHelp(words,i,i+1,i+2,i+3);
}

std::string StrFormat(wordFormat words){
	
	//Validate data
	
	if(words.size() < 1)
		return "";
	
	else if(words.size() == 1)
		return words[0];
		
	else if(words[0].size() < 4)
		return words[0];
	
	//Variables
	
	auto len = words[0].size();
	
	//Word processing
	
	for(int i=0;i<=len-3;i++){
		
		if(!indexHelp(words,i))
			return "";
			
		//We has to update the len value, because after some modification at main string, the len will change
		//and we need to analyze the whole word even if it's bigger
		
		else
			len = words[0].size();
	}
	
	//Return new formated word
	
	return words[0];
	
	//Usage sample, std::string URL StrFormat(wordFormat{"www.{0}.{1}","google","com"}); it will return www.google.com
}
