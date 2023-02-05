using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NBitcoin.DataEncoders;
using NNostr.Client;
using Relay.Data;
using Relay.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Relay.Pages
{
    public class IndexModel : PageModel
    {

        private readonly IDbContextFactory<RelayDbContext> _dbContextFactory;
        private readonly ILogger<IndexModel> _logger;
        private readonly IOptions<RelayOptions> _options;
        private readonly RelayDbContext _context;

        public IndexModel(IDbContextFactory<RelayDbContext> dbContextFactory, ILogger<IndexModel> logger,
            IOptions<RelayOptions> options)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _options = options;

            _context = _dbContextFactory.CreateDbContext();
        }
        public void OnGet(string? message)
        {
            ViewData["message"] = message;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                var pubkey = Request.Form["pubkey"].ToString();
                pubkey = pubkey.ToLower();
                pubkey = pubkey.Replace(" ", "");
                var bech32 = Encoders.Bech32("npub");
                var encodingType = Bech32EncodingType.BECH32;
                var hexKey = bech32.DecodeDataRaw(pubkey, out encodingType);
                var fromWords = ConvertBits(hexKey, 5, 8, false);
                var hexString = fromWords.ToHex();

                if (pubkey != null)
                {
                    var exists = _context.Whitelist.Any(a => a.PubKey == hexString);
                    if (exists)
                    {
                        return RedirectToPage("./Index", new { message = "key already on whitelist" });
                    }
                    else
                    {
                        _context.Whitelist.Add(new Whitelist { PubKey = hexString });
                        await _context.SaveChangesAsync();

                        return RedirectToPage("./Index", new { message = "key successfully added to whitelist" });
                    }
                }
            }catch(Exception ex)
            {
                return RedirectToPage("./Index", new { message = "error: "+ex.Message });
            }
            return RedirectToPage("./Index", new { message = "key empty" });
        }

        public byte[] ConvertBits(byte[] data, int fromBits, int toBits, bool pad = true)
        {
            var acc = 0;
            var bits = 0;
            var maxv = (1 << toBits) - 1;
            var ret = new List<byte>(64);
            foreach (var value in data)
            {
                if ((value >> fromBits) > 0)
                    throw new FormatException("Invalid Bech32 string");
                acc = (acc << fromBits) | value;
                bits += fromBits;
                while (bits >= toBits)
                {
                    bits -= toBits;
                    ret.Add((byte)((acc >> bits) & maxv));
                }
            }
            if (pad)
            {
                if (bits > 0)
                {
                    ret.Add((byte)((acc << (toBits - bits)) & maxv));
                }
            }
            else if (bits >= fromBits || (byte)(((acc << (toBits - bits)) & maxv)) != 0)
            {
                throw new FormatException("Invalid Bech32 string");
            }
            return ret.ToArray();
        }

    }
}
