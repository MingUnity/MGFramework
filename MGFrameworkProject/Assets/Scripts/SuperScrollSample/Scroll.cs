using MGFramework;
using MGFramework.UIModule;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    private class Parser : SuperScrollView.INodeParser
    {
        public void Parse(ISuperScrollNode node, ISuperScrollNodeData data)
        {
            (data as ScrollData).path.Bind(val => (node as ScrollNode).TexKey = val);
        }
    }

    private class Factory : ISuperNodeFactory
    {
        private Transform _template;
        public Factory(Transform template)
        {
            this._template = template;
        }
        public ISuperScrollNode Create()
        {
            ScrollNode scrollNode = new ScrollNode();
            Transform trans = Transform.Instantiate(_template, _template.parent);
            trans.gameObject.SetActive(true);
            scrollNode.Create(trans);
            return scrollNode;
        }

        public void Recycle(ISuperScrollNode node)
        {
            ScrollNode n = node as ScrollNode;
            n.Active = false;
        }
    }

    private SuperScrollView _scroll;

    //黑红白蓝白红黑蓝
    private ScrollData[] datas = new ScrollData[8]
    {
         new ScrollData(){  path = BindProperty<string>.Get("black")},
         new ScrollData(){  path =BindProperty<string>.Get( "red")},
         new ScrollData(){  path = BindProperty<string>.Get("white")},
         new ScrollData(){  path = BindProperty<string>.Get("blue")},
         new ScrollData(){  path = BindProperty<string>.Get("white")},
         new ScrollData(){  path = BindProperty<string>.Get("red")},
         new ScrollData(){  path = BindProperty<string>.Get("black")},
         new ScrollData(){  path = BindProperty<string>.Get("blue")}
    };

    private void Awake()
    {
        _scroll = this.GetComponent<SuperScrollView>();
    }

    private void Start()
    {
        Transform template = this.transform.Find("Viewport/Content/Template");
        template.gameObject.SetActive(false);
        _scroll.RefreshNodes(datas, new Factory(template), new Parser(),5);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _scroll.ScrollLast();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _scroll.ScrollNext();
        }
    }
}
