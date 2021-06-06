#include<iostream>
#include<conio.h>
using namespace std;

//Used to storing different data types in the same memory location
//We'll use it to convert float to long.

union show
{
    long f,f2;
};

//Main funtion.

int main(){
	
	system("mode con cols=48 lines=24");
	
	MAIN:
		
	//Clear console.
	
	system("cls");
	
	//Declaring variables.
	
	int read=0, year=0,month=0,week=0,days=0,b1=1,b2=0,temp=0,aux=1;
	float yD=0,mD=0,mD2=0,wD=0,dD=0,hD=0,minD=0,secD=0;
	float aux2=0;
	
	//Some settings for the console, as dimensions and title.
	
	system("color 0f && title Day's processor v1.0 Starlyn1232");
	
	//Here we input the days to variable "read".
	
	cout << "\nHow many days we'll process?: ";
	cin >> read;
	
	//Variable "aux2" will save "read"'s info, why? Because
	//we need the original value for the detailed info
	//processing.
	
	aux2=read;
	
	//As said before, here we are processing detailed info
	//keeping original value untouched.
	
	yD = aux2/365;
	mD = aux2;
	
	while(mD>=1){
		if(aux==2){
			mD-=28;
			mD2++;
		}
		if((aux%2!=0)&&(aux!=2)){
			mD-=31;
			mD2++;
			if(aux==8){
				mD-=31;
				mD2++;
				while((mD>=31)&&(aux!=13)){
					if((aux%2!=0)&&(mD>=31)){
						mD-=31;
						aux++;
						mD2++;
					}
				if((aux%2==0)&&(aux!=2)&&(mD>=30)){
					mD-=30;
					aux++;
					mD2++;
					}
				}
				if(aux==13){
					aux=1;
				}
			}
		}
		if((aux%2==0)&&(aux!=2)){
			mD-=30;
			mD2++;
		}
		aux++;
		if(aux==13){
			aux=1;
		}
		if((mD<=28)&&(aux!=2)&&(aux%2!=0)){
			if(mD!=1){
				mD2+=(mD/31);
				break;
			}
			break;
		}
		if((mD<=28)&&(aux!=2)&&(aux%20)&&(mD!=1)){
			if(mD!=1){
				mD2+=(mD/30);
				break;
			}
			break;
		}
	}
	
	wD = aux2/7;
	dD = aux2;
	hD = 24;
	hD *= aux2;
	
	//For minutes and seconds, the output is too long
	//to show all info perfectly, so, let's convert it.
	
	minD = 1440;
	minD *= aux2;
	
	//float to long convert.
	
	show show1;
	show1.f = minD;
	
	//float to long convert.
	
	secD = 86400;
	secD *= aux2;
	
	show show2;
	show2.f2 = secD;
	
	//Here we back to main value (from "read), first
	//we verify if it has at least 365 as value, this
	//means, at least one year, after that, let's
	//process it.
	
	if(read>=365){
		while(read>=365){
			if(b1%2==0){
				//Here we verify, b2 is going to increase just
				//is b1 is even, and b1 would increase after 
				//2 years, and after b1 increase twice (that means
				//a leap-year), the "read"'s value will be n-1, and
				//b2 will return to 0, and restart the verification
				//with the same idea, verify leap-years! (at least that's
				//the plan)
				b2++;
				
				if(b2==2){
					read-=1;
					b2=0;
				}
			}
			b1++;
			if(read>=365){
				read-=365;
				year++;
			}
			else{
				break;
			}
		}
	}
	
	if(read>28){
		//This one is not so easy, why? we need
		//to verify which months have, you know, 28
		//,30 and 31 days, well, it could just be
		//done using some declared variables, 12 ones
		// but, to make it more logical, I decided to
		//use just conditional.
		aux=1;
		while(read>=28){
			//Here we verify the first position (January)
			//, it must has 31 days at least and be at
			//position 1 (aux == month's order in this
			//case), if it's less than 31, then it's just
			//will set the values: weeks are now 4, (28/7)
			//and read will has this value: 2 for example.
			if((read<=30)&&(aux==1)){
				week+=4;
				read-=28;
				break;
			}
			if(aux==2){
				//Here we verify that month's order is at
				//second position, it means February, so
				//, this specific month is the only one with
				//28 days, so, we process it, we subtract
				//28 days, aux increase once and "month"'s
				//value increase also.
				read-=28;
				aux++;
				month++;
			}
			
			if((aux%2!=0)&&(aux!=2)&&(read>=31)){
				//Here we verify that, "aux"'s values is or
				//isn't even, but, why? We'll use this just
				//to know which month has 31 days and which
				//has 30 days, here we have also aux!=2
				//to avoid the verification for February
				//, as we know that it was coded at previous
				//conditional, here we substract 31 from "read".
				read-=31;
				aux++;
				month++;
				if(aux==8){
					read-=31;
					month++;
					aux++;
					//What has special that "aux" value is 8?
					//Well, it's needed to know when is August
					// because, after July (31 days's month),
					//August has same days. (it's also a 31 days's month)
					//and the verification continue.
					while(read>=31){
						if((aux%2!=0)&&(aux!=2)&&(read>=31)){
							read-=31;
							aux++;
							month++;
						}
						if((aux%2==0)&&(aux!=2)&&(read>=30)){
							read-=30;
							aux++;
							month++;
						}
					}
					read++;
					//We'll use break info because from here,
					//we don't need process it at month's sesion
					//and it'll continue at weeks's confitional.
					break;
				}
			}
			if((aux%2==0)&&(aux!=2)&&(read>=30)){
				//We'll use "aux"'s value again just
				//to know which month has 31 days and which
				//has 30 days, here we substract 30 from "read".
				read-=30;
				aux++;
				month++;
			}
		}
	}
	
	if(read>=7){
		//This is simple, while read's value is higher than
		//7, it will substract "read"'s value and increase
		//"week"'s value.
		while(read>=7){
			week++;
			read-=7;
		}
	}
	if(read>=1){
		//This is simple also, while "read"'s value is higher than
		//1, it will substract "read"'s value and increase
		//"days"'s value.
		while(read>=1){
			days++;
			read-=1;
		}
	}
	
	cout << "\nGeneral Info: " << endl;
	
	if(year!=0){
		cout << "\nYears: " << year;
	}
	if(month!=0){
		cout << "\nMonth: " << month;
	}
	
	if(week!=0){
		cout << "\nWeek: " << week;
	}
	if(days!=0){
		cout << "\nDays: " << days;
	}
	
	cout << "\n\nEach Info: " << endl;
	
	if(aux2!=0){
		cout << "\nYears: " << yD;
	}
	if(aux2!=0){
		cout << "\nMonth: " << mD2;
	}
	
	if(aux2!=0){
		cout << "\nWeek: " << wD;
	}
	if(aux2!=0){
		if(days==0){
			cout << "\nDays: " << dD;
		}
		cout << "\nHours: " << hD;
		cout << "\nMinutes: " << show1.f;
		cout << "\nSeconds: " << show2.f;
	}
	
	cout << "\n\n---------------------";
	cout << "\nPress Enter to restart.";
	getch();
	goto MAIN;
	
	return 0;
}
