﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hugo_LAND.Client.Services {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Services.ICompteJoueurService")]
    public interface ICompteJoueurService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICompteJoueurService/ConnexionUtilisateur", ReplyAction="http://tempuri.org/ICompteJoueurService/ConnexionUtilisateurResponse")]
        string ConnexionUtilisateur(string NomUtilisateur, string MotDePasse);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICompteJoueurService/ConnexionUtilisateur", ReplyAction="http://tempuri.org/ICompteJoueurService/ConnexionUtilisateurResponse")]
        System.Threading.Tasks.Task<string> ConnexionUtilisateurAsync(string NomUtilisateur, string MotDePasse);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICompteJoueurServiceChannel : Hugo_LAND.Client.Services.ICompteJoueurService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CompteJoueurServiceClient : System.ServiceModel.ClientBase<Hugo_LAND.Client.Services.ICompteJoueurService>, Hugo_LAND.Client.Services.ICompteJoueurService {
        
        public CompteJoueurServiceClient() {
        }
        
        public CompteJoueurServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CompteJoueurServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CompteJoueurServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CompteJoueurServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string ConnexionUtilisateur(string NomUtilisateur, string MotDePasse) {
            return base.Channel.ConnexionUtilisateur(NomUtilisateur, MotDePasse);
        }
        
        public System.Threading.Tasks.Task<string> ConnexionUtilisateurAsync(string NomUtilisateur, string MotDePasse) {
            return base.Channel.ConnexionUtilisateurAsync(NomUtilisateur, MotDePasse);
        }
    }
}
