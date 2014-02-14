#include <stdio.h>
#include <linux/i2c-dev.h>
#include <fcntl.h>
#include <string.h>
#include <sys/ioctl.h>
#include <unistd.h>
#include <errno.h>
#include <stdio.h>
#include "libNativeI2C.h"

int openBus (char* busFileName)
{
    int r = open (busFileName, O_RDWR);
    printf("Opening %2s , result %i", busFileName, r);
    return r;
}

int closeBus (int busHandle)
{
    return  close (busHandle);
}

int writeBytes (int busHandle, int addr, byte* buf, int len)
{
	if (ioctl (busHandle, I2C_SLAVE, addr) < 0)
		return -1;

	if (write (busHandle, buf, len) != len)
		return -2;

	return len;
}

int readBytes (int busHandle, int addr, byte* buf, int len)
{
	if (ioctl (busHandle, I2C_SLAVE, addr) < 0)
		return -1;

	int n= read (busHandle, buf, len);

	return n;
}