using FriendsGamesTools.ECSGame;
using FriendsGamesTools.UI;

namespace DrawSpell
{
    public class DrawSpellWindows : Windows
    {
        protected override bool backShown => base.backShown && !Windows.Get<MainMenuWindow>().shown;
    }
}