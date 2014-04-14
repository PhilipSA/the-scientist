﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;

namespace TileContent
{
    /// <summary>
    /// Hjälp klasser för hantering av konversationer och konversationsblock.
    /// </summary>
    public class ScriptContent
    {
        public Collection<ConversationContent> Conversations =
            new Collection<ConversationContent>();
    }

    public class ConversationContent
    {
        public string Name;
        public string Text;
        public Collection<ConversationHandlerContent> Handlers = new Collection<ConversationHandlerContent>();
      
    }

    public class ConversationHandlerContent
    {
        public string caption;
        public string action;
        public object[] actionParameters;
     
    }

}