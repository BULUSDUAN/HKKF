using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace hkkf.Common.Validations
{
    /// <summary>
    /// 身份证号码
    /// </summary>
    public class IdentityNumerAttribute: ValidationAttribute
    {
        public bool IsInt(string numberString)
        {
            Regex rCode = new Regex("^[+-]?\\d+(\\d+)?$ ");
            if (rCode.IsMatch(numberString))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string strSex = null;

        public override bool IsValid(object value)
        {
            string strError = null;
            var s = value as string;
            if (string.IsNullOrEmpty(s)) return true;

            int intIdlength = s.Trim().Length;

            string strIDNONew = null;

            if ((intIdlength != 15) && (intIdlength != 18))
            {
                strError = "身份证号长度不合法";
                ErrorMessage = strError;
                return false;
            }

            if (intIdlength == 18)
            {
                strIDNONew = s.Substring(0, s.Length - 1);
                if (this.IsInt(strIDNONew))
                {
                    strError = "输入格式不正确";
                    ErrorMessage = strError;
                    return false;
                }
            }
            else if (intIdlength == 15)
            {
                if (this.IsInt(s))
                {
                    strError = "输入格式不正确";
                    ErrorMessage = strError;
                    return false;
                }
            }

            if (intIdlength == 18)
            {
                int[] w = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                string[] a = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };
                int i, j, d;
                d = 0;
                for (i = 0; i < 17; i++)
                {
                    j = Convert.ToInt32(s.Substring(i, 1)) * w[i];
                    d = d + j;
                }
                d = d % 11;
                if (s.Substring(17, 1).ToString().Trim() != a[d])
                {
                    if (s[17].ToString().Trim() == "X")
                    {
                        if (s[17].ToString().Trim().ToLower() != a[d])
                        {
                            strError = "身份证不合法，校验码错误。应为" + a[d];
                            ErrorMessage = strError;
                            return false;
                        }
                    }
                    else
                    {
                        strError = "身份证不合法，校验码错误。应为" + a[d];
                        ErrorMessage = strError;
                        return false;
                    }
                }
            }

            int intSex;
            if (intIdlength == 18)
            {
                intSex = Convert.ToInt32(s.Substring(intIdlength - 2, 1));
                if ((intSex == 1) || (intSex == 3) || (intSex == 5) || (intSex == 7) || (intSex == 9))
                    strSex = "男";
                else
                    strSex = "女";


                int strYear = Convert.ToInt32(s.Substring(6, 4));
                int strMonth = Convert.ToInt32(s.Substring(10, 2));
                int strDay = Convert.ToInt32(s.Substring(12, 2));
                Boolean blnRunNian = false;
                if ((strYear % 4 == 0) && (strYear % 100 != 0))
                    blnRunNian = true;
                else if (strYear % 400 == 0)
                    blnRunNian = true;

                if ((strYear > 1901) && (strYear < 2050) && (strMonth <= 12) && (strMonth > 0) && (strDay > 0))
                {
                    switch (blnRunNian)
                    {
                        case true:
                            if ((strMonth == 1) || (strMonth == 3) || (strMonth == 5) || (strMonth == 7) || (strMonth == 8) || (strMonth == 10) || (strMonth == 12))
                            {
                                if (strDay > 31)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if ((strMonth == 4) || (strMonth == 6) || (strMonth == 9) || (strMonth == 11))
                            {
                                if (strDay > 30)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if (strMonth == 2)
                            {
                                if (strDay > 29)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            break;
                        case false:
                            if ((strMonth == 1) || (strMonth == 3) || (strMonth == 5) || (strMonth == 7) || (strMonth == 8) || (strMonth == 10) || (strMonth == 12))
                            {
                                if (strDay > 31)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if ((strMonth == 4) || (strMonth == 6) || (strMonth == 9) || (strMonth == 11))
                            {
                                if (strDay > 30)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if (strMonth == 2)
                            {
                                if (strDay > 28)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    strError = "身份证日期错误";
                    ErrorMessage = strError;
                    return false;
                }
            }
            if (intIdlength == 15)
            {
                intSex = Convert.ToInt32(s.Substring(intIdlength - 1, 1));
                if ((intSex == 1) || (intSex == 3) || (intSex == 5) || (intSex == 7) || (intSex == 9))
                    strSex = "男";
                else
                    strSex = "女";
                string strYear = s.Substring(6, 2);
                int strYear2 = Convert.ToInt32("19" + strYear);
                int strMonth = Convert.ToInt32(s.Substring(8, 2));
                int strDay = Convert.ToInt32(s.Substring(10, 2));
                int isRunNian = int.Parse(strYear) % 4;
                Boolean blnRunNian = false;
                if ((int.Parse(strYear) % 4 == 0) && (int.Parse(strYear) % 100 != 0))
                    blnRunNian = true;
                else if (int.Parse(strYear) % 400 == 0)
                    blnRunNian = true;

                if ((strYear2 > 1901) && (strYear2 < 2050) && (strMonth <= 12) && (strMonth > 0) && (strDay > 0))
                {
                    switch (blnRunNian)
                    {
                        case true:
                            if ((strMonth == 1) || (strMonth == 3) || (strMonth == 5) || (strMonth == 7) || (strMonth == 8) || (strMonth == 10) || (strMonth == 12))
                            {
                                if (strDay > 31)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if ((strMonth == 4) || (strMonth == 6) || (strMonth == 9) || (strMonth == 11))
                            {
                                if (strDay > 30)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if (strMonth == 2)
                            {
                                if (strDay > 29)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            break;
                        case false:
                            if ((strMonth == 1) || (strMonth == 3) || (strMonth == 5) || (strMonth == 7) || (strMonth == 8) || (strMonth == 10) || (strMonth == 12))
                            {
                                if (strDay > 31)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if ((strMonth == 4) || (strMonth == 6) || (strMonth == 9) || (strMonth == 11))
                            {
                                if (strDay > 30)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            else if (strMonth == 2)
                            {
                                if (strDay > 28)
                                {
                                    strError = "身份证日期错误";
                                    ErrorMessage = strError;
                                    return false;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    strError = "身份证日期错误";
                    ErrorMessage = strError;
                    return false;
                }
            }
            return true;
        }

        public string ChangePersonID15To18(string personIdentity)
        {
            personIdentity = personIdentity.Trim();
            if ((IsInt(personIdentity) == false) || (personIdentity.Trim().Length != 15))
            {
                return personIdentity.Trim();
            }

            int[] w = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            string[] a = { "1", "0", "x", "9", "8", "7", "6", "5", "4", "3", "2" };

            int i, j, s;
            string newID;
            newID = personIdentity.Substring(0, 6) + "19" + personIdentity.Substring(6, personIdentity.Trim().Length);
            s = 0;
            for (i = 0; i < 17; i++)
            {
                j = int.Parse(newID.Substring(i, 1)) * w[i];
                s = s + j;
            }
            s = s % 11;
            return newID + a[s];
        }
    }

}
