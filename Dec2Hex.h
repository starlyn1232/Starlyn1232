//Dec to Hex conversor by Starlyn1232 (C++)

#ifndef string
#include<string>
#endif

std::string Dec2Hex(int dec){
	
	char hex[16] = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
	
	std::string temp("");
	
	while(dec>0){
		temp = hex[(dec%16)] + temp;
		dec /= 16;
	}
	
	return temp;
	
	//Usage sample : std::string hex = Dec2Hex(num);
}
