CSHARP_COMPILER = csc

TARGET = xmodem.exe

MAIN_CLASS = Program.cs
PARTIAL_CLASS = XModem.cs crc16.cs
OPT = -optimize+

all: $(TARGET)

$(TARGET) :
	$(CSHARP_COMPILER) $(OPT) $(MAIN_CLASS) $(PARTIAL_CLASS)

clean:
	rm *.exe