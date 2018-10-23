using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolpathLib
{
    public enum FeedrateUnits
    {
        MmPerSec,
        MmPerMin,
        InPerSec,
        InPerMin,
        SecPerMove,
        InvMinPerMove
    }
    
    public class Feedrate

    {
        FeedrateUnits _units;
        bool _inverted;
        double _value;
        public bool Inverted 
        { 
            get
            {
                return _inverted;
            } 
            set
            {
                _inverted = value;
            }
        }
        public double Value
        {
            get
            { return _value; }
            set
            { _value = value; }
        }
        public FeedrateUnits Units 
        {
           get
           {
            return _units;
           }
            set
            {
                _units = value;               
            }
        }
        public double MoveTimeSeconds(double moveLength)
        {
            double result = 0;
            if (_value != 0)
            {               
                    switch (_units)
                    {
                        case FeedrateUnits.InvMinPerMove:
                            result = (moveLength * _value) / 60;
                            break;
                        case FeedrateUnits.SecPerMove:
                            result = _value;
                            break;
                        case FeedrateUnits.MmPerSec:
                        case FeedrateUnits.InPerSec:
                            result = moveLength / _value;
                            break;
                        case FeedrateUnits.MmPerMin:
                        case FeedrateUnits.InPerMin:
                        default:
                            result = (moveLength / _value) * 60;
                            break;
                    }           
            }
            return result;
        }
        public Feedrate()
        {
            Inverted = false;
            Value = 0;
            Units = FeedrateUnits.InPerMin;
        }
        public Feedrate(FeedrateUnits units)
        {
            _units = units;
            switch (_units)
            {
                case FeedrateUnits.InvMinPerMove:  
                case FeedrateUnits.SecPerMove:
                    _inverted = true;                  
                    break;
                case FeedrateUnits.MmPerSec:
                case FeedrateUnits.InPerSec:       
                case FeedrateUnits.MmPerMin:
                case FeedrateUnits.InPerMin:
                default:
                    _inverted = false;
                    break;
            }

        }
        public Feedrate( Feedrate f)
        {
            _inverted = f.Inverted;
            _units = f.Units;
            _value = f.Value;
        }
    }
}
