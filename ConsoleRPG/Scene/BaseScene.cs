using System.Data;

namespace ConsoleRPG
{
    // 모든 씬의 부모 클래스
    public class BaseScene
    {
        public virtual string Title { get; protected set; }
        protected List<ActionOption> Options { get; set; } = new();
        protected int selectOptionIndex = 0;
        protected Command lastCommand = Command.None;

        public virtual void EnterScene() { } // 씬 진입 시 기능구현
        public virtual void NextScene() { }  // 다음 씬 넘어가기 위한 기능구현
        protected virtual void DrawScene() { } // 씬을 보여주는 기능구현

        protected void GetInput()
        {
            lastCommand = Command.None;
            lastCommand = Console.ReadKey(true).Key switch
            {
                ConsoleKey.UpArrow => Command.MoveTop,
                ConsoleKey.DownArrow => Command.MoveBottom,
                ConsoleKey.LeftArrow => Command.MoveLeft,
                ConsoleKey.RightArrow => Command.MoveRight,
                ConsoleKey.Enter => Command.Interact,
                ConsoleKey.Escape => Command.Exit,
                _ => Command.None,
            };

            OnCommand(lastCommand);
        }
        void OnCommand(Command command)
        {
            switch (command)
            {
                case Command.MoveTop: OnCommandMoveTop(); break;
                case Command.MoveBottom: OnCommandMoveBottom(); break;
                case Command.MoveLeft: OnCommandMoveLeft(); break;
                case Command.MoveRight: OnCommandMoveRight(); break;
                case Command.Interact: OnCommandInteract(); break;
                case Command.Exit: OnCommandExit(); break;
            }
        }
        protected virtual void OnCommandMoveTop()
        {
            if (selectOptionIndex > 0)
                selectOptionIndex--;
        }
        protected virtual void OnCommandMoveBottom()
        {
            if (selectOptionIndex < Options.Count - 1)
                selectOptionIndex++;
        }
        protected virtual void OnCommandMoveLeft() { }
        protected virtual void OnCommandMoveRight() { }
        protected virtual void OnCommandInteract()
        {
            if (Options.Count > 0)
                Options[selectOptionIndex].Excute();
        }
        protected virtual void OnCommandExit()
        {
            if (Managers.Scene.PrevScene != null)
                Managers.Scene.GetOption("Back").Excute();
        }
    }
}
