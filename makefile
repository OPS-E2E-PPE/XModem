#make -n 명령어로 빌드없이 실행 명령어만 볼 수 있다.

CSHARP_COMPILER = csc

TARGET = xmodem.exe
TARGET_DIR = .\XModem_Project\\

MAIN_CLASS = $(TARGET_DIR)Program.cs
PARTIAL_CLASS = $(TARGET_DIR)XModem.cs $(TARGET_DIR)crc16.cs
OPT = -optimize+ 
#-define:DEBUG
all:
	$(CSHARP_COMPILER) -out:$(TARGET) $(OPT) $(MAIN_CLASS) $(PARTIAL_CLASS) 

clean:
	rm *.exe