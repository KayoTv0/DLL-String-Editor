# DLL String Editor

A simple and fast tool to extract, view, and edit strings inside DLL or binary files.

## Features

* Extract ASCII and UTF-16 strings
* Edit strings directly inside the file
* Save modifications safely
* Simple and lightweight interface
* Fast string scanning

## Preview

<img width="744" height="407" alt="preview" src="https://github.com/user-attachments/assets/b08108a6-1030-4806-9431-fe765669df54" />

## How to Use

1. Launch the application
2. Click **Open** and select your DLL or binary file
3. Browse detected strings in the list
4. Select a string to view/edit it
5. Click **Apply** to modify the string in memory
6. Click **Save** to write changes to file

## Important Notes

* Edited strings must not exceed the original length
* The tool replaces bytes directly (no resizing)
* Always keep a backup of your original file

## Build

Open the project in Visual Studio and build it:

```
Build > Build Solution
```

Or run directly:

```
Ctrl + F5
```

## Release

The release version includes:

* `.exe`
* `.dll`
* `.runtimeconfig.json`

Or you can publish a single executable using:

```
Publish > Self-contained > Single file
```
