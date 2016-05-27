#include "des.h"
#include "Base64.h"

class Encrypt{
public :     
	Encrypt();
	~Encrypt();
    char* encrypt (unsigned char* key, char* data); 
    char* decrypt (unsigned char* key, char* data);
private:
    CBase64 base;
	BYTE* pCip;
};
