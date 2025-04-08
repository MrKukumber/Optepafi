using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Optepafi.Models.Utils.EsriBilParser;

public class EsriBilParser
{
    private string _fileName = "";

    private string _biltype;
    private string _source; 

    private string _byteorder;
    private int _nbits;
    private double _xdim;
    private double _ydim;
    private int _ncols;
    private int _nrows;
    private int _nbands;
    private double _ulxmap;
    private double _ulymap;
    
    public string FileName => _fileName;
    
    public int NBands => _nbands;
    public int NRows => _nrows;
    public int NColumns => _ncols;
    
    
    
    // reading bil header parameters only, and fill in properties
    public void ReadBilHeader(string FileName)
    {
        _fileName = FileName;
        _ncols = -1;
        _nrows = -1;
        _nbands = -1;
        string fName = Path.ChangeExtension(FileName, ".hdr");
        //string fName =System.IO.Path.GetDirectoryName(FileName)+ "\\" + System.IO.Path.GetFileNameWithoutExtension(FileName) + ".hdr";

        try {
            using (FileStream fs = new FileStream(fName, FileMode.Open, FileAccess.Read))
            {
                StreamReader sr = new StreamReader(fs);
                while
                    (!sr.EndOfStream) //reading header file (FileName) line by line, and getting parameters by getBilParameters
                {
                    string line = sr.ReadLine();
                    if (line != "")
                        GetBilParameters(line);
                }
            }
        } catch (FileNotFoundException e) {
            throw new FileNotFoundException(e.Message, e);
        } catch (DirectoryNotFoundException e) {
            throw new DirectoryNotFoundException(e.Message, e);
        } catch (IOException e) {
            throw new IOException(e.Message, e);
        }
        if (_ncols == -1 || _nrows == -1 || _nbands == -1)
            throw new FormatException("Invalid header file format. It does not contain information about number of columns, rows or bands of bil file.");
    }
    
    //get bil parameters from the header file line by line
    private void GetBilParameters(string param)
    {
        string p = param.ToLower(CultureInfo.InvariantCulture).Trim();
        
        // if (p.Contains("arc/info"))
        // {
            // _biltype = "Arc/Info";
            // _source = param;
        // }

        string[] l = p.Split(' ', StringSplitOptions.RemoveEmptyEntries); //p is two dimensional string array. First component is the name :l[0], and the second one is the value: l[1]
        if (l[0] == "byteorder") _byteorder = l[1];
        if (l[0] == "nbits") _nbits = int.Parse(l[1]);
        if (l[0] == "xdim") _xdim = double.Parse(l[1], CultureInfo.InvariantCulture);
        if (l[0] == "ydim") _ydim = double.Parse(l[1], CultureInfo.InvariantCulture);
        if (l[0] == "ncols") _ncols = int.Parse(l[1]);
        if (l[0] == "nrows") _nrows = int.Parse(l[1]);
        if (l[0] == "nbands") _nbands = int.Parse(l[1]);
        if (l[0] == "ulxmap") _ulxmap = double.Parse(l[1], CultureInfo.InvariantCulture);
        if (l[0] == "ulymap") _ulymap = double.Parse(l[1], CultureInfo.InvariantCulture);
    }

    public int[,] GetOneBandInts(int whichBand)
    {
        if (_fileName == "") throw(new InvalidOperationException("ReadBilHeader must be called first."));
        
        int[,] pixelsOut=new int[_nrows,_ncols];

        try
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                int startPos = 0;
                for (int i = 0; i < _nrows; i++)
                {
                    startPos = (_nbands * i + whichBand) * _ncols; //  _nbands * _ncols * i  + whichBand * _ncols
                    fs.Position = startPos;
                    for (int j = 0; j < _ncols; j++)
                        if (_nbits == 16)
                            pixelsOut[i, j] = br.ReadInt16();
                        else
                            pixelsOut[i, j] = br.ReadByte();

                }
            }
        }
        catch (FileNotFoundException e) { throw new FileNotFoundException(e.Message, e); }
        catch (DirectoryNotFoundException e) { throw new DirectoryNotFoundException(e.Message, e); }
        catch (EndOfStreamException e) { throw new EndOfStreamException(e.Message, e); }
        catch (IOException e) { throw new IOException(e.Message, e); } 
        return pixelsOut;
    }

    public int[][,] GetAllBandsInts()
    {
        if (_fileName == "") throw(new InvalidOperationException("ReadBilHeader must be called first."));
        
        int[][,] pixelsOut=new int[_nbands][,];
        for (int i = 0; i < _nbands; ++i) pixelsOut[i]=new int[_nrows,_ncols];
        try
        {
            using (FileStream fs = new FileStream(_fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                for (int i = 0; i < _nrows; i++)
                for (int b = 0; b < _nbands; b++)
                for (int j = 0; j < _ncols; j++)
                    pixelsOut[b][i, j] = _nbits switch
                    {
                        32 => br.ReadInt32(),
                        16 => br.ReadInt16(),
                        _ => br.ReadByte(),
                    };
            }
        }
        catch (FileNotFoundException e) { throw new FileNotFoundException(e.Message, e); }
        catch (DirectoryNotFoundException e) { throw new DirectoryNotFoundException(e.Message, e); }
        catch (EndOfStreamException e) { throw new EndOfStreamException(e.Message, e); }
        catch (IOException e) { throw new IOException(e.Message, e); }
        return pixelsOut;
        
    }
}