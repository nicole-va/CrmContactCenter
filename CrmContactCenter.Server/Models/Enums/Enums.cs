namespace CrmContactCenter.Server.Models.Enums
{
    public enum CustomerStatus
    {
        activo,
        inactivo
    }

    public enum AccountStatus
    {
        pendiente,
        pagado,
        vencido
    }

    public enum InteractionChannel
    {
        llamada,
        whatsapp,
        email
    }

    public enum InteractionResult
    {
        contactado,
        no_responde,
        promesa_de_pago,
        pago_realizado,
        numero_incorrecto,
        buzon_de_voz
    }

    public enum FollowUpStatus
    {
        pendiente,
        completado,
        cancelado
    }
}