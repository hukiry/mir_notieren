////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace Hukiry.MDV
{
    public interface IActions
    {
        Texture FetchImage( string url );
        void    SelectPage( string url );
    }
}

