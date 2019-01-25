using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models.Role
{
        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Root
        {

            private RootMenu[] menuField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Menu")]
            public RootMenu[] Menu
            {
                get
                {
                    return this.menuField;
                }
                set
                {
                    this.menuField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootMenu
        {
            //[System.Xml.Serialization.XmlElementAttribute("Submenu")]
            //public List<RootMenuSubmenu> submenuField { get; set; }
            private RootMenuSubmenu[] submenuField;

            private string nameField;

            private string textField;

            private bool checkField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Submenu")]
            public RootMenuSubmenu[] Submenu
            {
                get
                {
                    return this.submenuField;
                }
                set
                {
                    this.submenuField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool check
            {
                get
                {
                    return this.checkField;
                }
                set
                {
                    this.checkField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootMenuSubmenu
        {

            private RootMenuSubmenuEntity[] entityField;

            private string nameField;

            private string textField;

            private bool checkField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Entity")]
            public RootMenuSubmenuEntity[] Entity
            {
                get
                {
                    return this.entityField;
                }
                set
                {
                    this.entityField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool check
            {
                get
                {
                    return this.checkField;
                }
                set
                {
                    this.checkField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootMenuSubmenuEntity
        {

            private RootMenuSubmenuEntityAction[] actionField;

            private string nameField;

            private string textField;

            private string pathField;

            private bool checkField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Action")]
            public RootMenuSubmenuEntityAction[] Action
            {
                get
                {
                    return this.actionField;
                }
                set
                {
                    this.actionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string path
            {
                get
                {
                    return this.pathField;
                }
                set
                {
                    this.pathField = value;
                }
            }


            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool check
            {
                get
                {
                    return this.checkField;
                }
                set
                {
                    this.checkField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class RootMenuSubmenuEntityAction
        {

            private string nameField;

            private string textField;

            private bool checkField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string text
            {
                get
                {
                    return this.textField;
                }
                set
                {
                    this.textField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public bool check
            {
                get
                {
                    return this.checkField;
                }
                set
                {
                    this.checkField = value;
                }
            }
        }
}