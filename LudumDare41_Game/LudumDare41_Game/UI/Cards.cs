using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare41_Game.UI {

    class Cards {

    }

    class Card {
        string title, desc, imgNameSrc, bgNameSrc;
        Texture2D cardImg, bg;
        bool isLoaded { get; }

        public Card(string title, string desc, string imgNameSrc, string bgNameSrc) { //dette er de forskjellige Card typene, som kan bli håndtert i Cards classen?
            this.title = title;
            this.desc = desc;
            this.imgNameSrc = imgNameSrc;
            this.bgNameSrc = bgNameSrc;
            isLoaded = false;
        }

        public void Load(ContentManager c) {
            cardImg = c.Load<Texture2D>("GUI/Cards/" + title + "/" + imgNameSrc);
            bg = c.Load<Texture2D>("GUI/Cards/" + title + "/" + bgNameSrc);
        }
    }
}
