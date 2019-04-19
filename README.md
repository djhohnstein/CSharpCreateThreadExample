# CreateThread Example

C# code to use CreateThread to run position independent code in the running process. This code is provided AS IS, and will not be supported.

## XOREncrypt

Binary to encrypt PIC using a sample key.

### Usage

```
.\XOREncrypt.exe path\to\pic.bin
```

## CreateThreadExample

Binary to run encrypted PIC from the XOREncrypt binary using CreateThread. To use, must embed encrypted.bin as a resource file in the project and select "Embedded Resource" as the build action.

### Usage
```
.\CreateThreadExample.bin
```